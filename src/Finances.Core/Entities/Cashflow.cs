using System;
using System.Linq;
using System.Collections.Generic;
using Finances.Core.Engines.Cashflow;
using Finances.Core.Interfaces;

namespace Finances.Core.Entities
{
    public class Cashflow : Entity
    {
        readonly IProjectionTransferGenerator projectionTransferGenerator;

        public Cashflow(IProjectionTransferGenerator projectionTransferGenerator)
        {
            this.projectionTransferGenerator = projectionTransferGenerator;

            CashflowBankAccounts = new List<CashflowBankAccount>();
        }

        public int CashflowId { get; set; }
        public string Name { get; set; }
        public decimal OpeningBalance { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime RecordCreatedDateTime { get; set; }
        public DateTime RecordUpdatedDateTime { get; set; }

        public List<CashflowBankAccount> CashflowBankAccounts { get; set; }

        //public void AddCashflowBankAccount(BankAccount a)
        //{
        //    CashflowBankAccounts.Add(new CashflowBankAccount() { BankAccount = a });
        //}


        public List<CashflowProjectionItem> GenerateProjection(
                        DateTime endDate,
                        decimal openingBalance,
                        decimal threshold,
                        IAggregatedProjectionItemsGenerator projectionMode
                        )
        {
            List<CashflowProjectionTransfer> cpts = projectionTransferGenerator.GenerateProjectionTransfers(this.CashflowBankAccounts, this.StartDate, endDate);

            List<CashflowProjectionItem> cpis = projectionMode.GenerateAggregatedProjectionItems(cpts); 

            ApplyBalancesAndThreshold(cpis, this.StartDate, openingBalance, threshold);

            return cpis;
        }


        List<CashflowProjectionItem> ApplyBalancesAndThreshold(List<CashflowProjectionItem> cashflowProjectionItems,
                                                                DateTime startDate,
                                                                decimal openingBalance,
                                                                decimal threshold)
        {

            cashflowProjectionItems.Insert(0, new CashflowProjectionItem()
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
}
