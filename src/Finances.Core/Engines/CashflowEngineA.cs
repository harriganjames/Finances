using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.Interfaces;
using Finances.Core.Entities;
using Finances.Core.Engines.Cashflow;

namespace Finances.Core.Engines
{
    public class CashflowEngineA : ICashflowEngineA
    {
        readonly IBankAccountRepository bankAccountRepository;
        readonly ITransferRepository transferRepository;

        public CashflowEngineA(
                        IBankAccountRepository bankAccountRepository,
                        ITransferRepository transferRepository)
        {
            this.bankAccountRepository = bankAccountRepository;
            this.transferRepository = transferRepository;
        }



        public List<CashflowProjectionItem> GenerateProjection(
                                List<CashflowBankAccount> accounts,
                                DateTime startDate,
                                DateTime endDate,
                                decimal openingBalance,
                                decimal threshold,
                                ProjectionModeEnum mode)
        {
            List<CashflowProjectionTransfer> cpts = this.GenerateProjectionTransfers(accounts, startDate, endDate);

            List<CashflowProjectionItem> cpis = GenerateAggregatedProjectionItems(cpts, mode);

            ApplyBalancesAndThreshold(cpis, startDate, openingBalance, threshold);

            return cpis;
        }



        private List<CashflowProjectionTransfer> GenerateProjectionTransfers(
                                List<CashflowBankAccount> accounts,
                                DateTime startDate,
                                DateTime endDate)
        {
            var cpts = new List<CashflowProjectionTransfer>();
            List<BankAccount> bankAccounts;

            // get list of bank accounts involved
            if (accounts.Count == 0)
                bankAccounts = this.bankAccountRepository.ReadList();
            else
            {
                bankAccounts = new List<BankAccount>();
                accounts.ForEach(cba => bankAccounts.Add(cba.BankAccount));
            }


            List<TransferDirection> transferDirections = GetTransferDirections(bankAccounts);


            // extrapolate date range.
            // loop each transfer
            foreach (var td in transferDirections)
            {
                Transfer t = td.Transfer;
                // loop compatible date range
                DateTime d = t.Schedule.StartDate < startDate ? t.Schedule.StartDate : startDate;
                while (d <= endDate && (t.Schedule.EndDate == null || d <= t.Schedule.EndDate))
                {
                    cpts.Add(new CashflowProjectionTransfer() { Date = d, TransferDirection = td });

                    if (t.Schedule.Frequency == "Monthly")
                        d = d.AddMonths(1);
                }
            }


            return cpts;
        }


        private List<TransferDirection> GetTransferDirections(List<BankAccount> bankAccounts)
        {
            var transferDirections = (from t in this.transferRepository.ReadList()
                                      let outBound = t.FromBankAccount != null && bankAccounts.Count(a => a.BankAccountId == t.FromBankAccount.BankAccountId) > 0
                                      let inBound = t.ToBankAccount != null && bankAccounts.Count(a => a.BankAccountId == t.ToBankAccount.BankAccountId) > 0
                                      where inBound ^ outBound  // xor == exclude internal transfers
                                        && t.IsEnabled
                                      select new TransferDirection()
                                      {
                                          Transfer = t,
                                          IsOutbound = outBound,
                                          IsInbound = inBound
                                      }
                ).ToList();

            return transferDirections;
        }



        List<CashflowProjectionItem> GenerateAggregatedProjectionItems(List<CashflowProjectionTransfer> cashflowProjectionTransfers, ProjectionModeEnum mode)
        {
            var cpis = new List<CashflowProjectionItem>();

            if (mode == ProjectionModeEnum.Detail)
            {
                foreach (var cpt in cashflowProjectionTransfers.OrderBy(c => c.Date))
                {
                    var cpi = new CashflowProjectionItem()
                    {
                        Period = cpt.Date.ToString("yyyy-MM-dd"),
                        PeriodStartDate = cpt.Date,
                        PeriodEndDate = cpt.Date,
                        Item =  (cpt.TransferDirection.Transfer.Category.Code=="NONE" ? "" : cpt.TransferDirection.Transfer.Category.Name + " -> ") + cpt.TransferDirection.Transfer.Name,
                        In = cpt.TransferDirection.IsInbound ? cpt.TransferDirection.Transfer.Amount : (decimal?)null,
                        Out = cpt.TransferDirection.IsOutbound ? cpt.TransferDirection.Transfer.Amount : (decimal?)null
                    };

                    cpis.Add(cpi);
                }
            }
            else if (mode == ProjectionModeEnum.MonthlySummary)
            {
                var x = from cpt in cashflowProjectionTransfers.OrderBy(c => c.Date)
                        group cpt by new
                        {
                            Year = cpt.Date.Year,
                            Month = cpt.Date.Month,
                            Item = (cpt.TransferDirection.Transfer.Category.Code == "NONE" ? cpt.TransferDirection.Transfer.Name : cpt.TransferDirection.Transfer.Category.Name),
                        }
                            into grp
                            let inAmount = grp.Sum(c => c.TransferDirection.IsInbound ? c.TransferDirection.Transfer.Amount : 0M)
                            let outAmount = grp.Sum(c => c.TransferDirection.IsOutbound ? c.TransferDirection.Transfer.Amount : 0M)
                            select new CashflowProjectionItem
                            {
                                Period = new DateTime(grp.Key.Year, grp.Key.Month, 1).ToString("yyyy-MM"),
                                PeriodStartDate = grp.Min(c => c.Date),
                                PeriodEndDate = grp.Max(c => c.Date),
                                Item = grp.Key.Item,
                                In = inAmount==0?(decimal?)null:inAmount,
                                Out = outAmount==0?(decimal?)null:outAmount
                            };

                cpis.AddRange(x);

            }


            return cpis;
        }



        List<CashflowProjectionItem> ApplyBalancesAndThreshold(List<CashflowProjectionItem> cashflowProjectionItems,
                                                                        DateTime startDate,
                                                                        decimal openingBalance,
                                                                        decimal threshold)
        {

            cashflowProjectionItems.Insert(0,new CashflowProjectionItem()
            {
                Period = startDate.ToString("yyyy-MM-dd"),
                Item = "Opening Balance",
                In = null,
                Out = null,
                Balance = openingBalance
            });

            // calculate the running balances


            var previousCpi = cashflowProjectionItems[0]; // start with opening balance item
            foreach (var cpi in cashflowProjectionItems.Skip(1))
            {
                if (previousCpi.Period != cpi.Period)
                    previousCpi.PeriodBalance = previousCpi.Balance;
                cpi.Balance = previousCpi.Balance + (cpi.In ?? 0M) - (cpi.Out ?? 0M);
                previousCpi = cpi;
            }
            previousCpi.PeriodBalance = previousCpi.Balance;

            // set the BalanceState for threshold
            foreach (var cpi in cashflowProjectionItems)
            {
                if (cpi.Balance < 0)
                    cpi.BalanceState = BalanceStateEnum.Negative;
                else if (cpi.Balance < threshold)
                    cpi.BalanceState = BalanceStateEnum.BelowThreshold;
                else
                    cpi.BalanceState = BalanceStateEnum.OK;
            }

            return cashflowProjectionItems;
        }



    }

    //class TransferDirection
    //{
    //    public Transfer Transfer { get; set; }
    //    public bool IsInbound { get; set; }
    //    public bool IsOutbound { get; set; }
    //}

    //class CashflowProjectionTransfer
    //{
    //    public DateTime Date { get; set; }
    //    public TransferDirection TransferDirection { get; set; }
    //}

}
