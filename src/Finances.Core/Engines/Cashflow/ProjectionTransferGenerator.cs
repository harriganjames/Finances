using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Finances.Core.Entities;
using Finances.Core.Interfaces;


namespace Finances.Core.Engines.Cashflow
{
    public class ProjectionTransferGenerator : IProjectionTransferGenerator
    {
        readonly IBankAccountRepository bankAccountRepository;
        readonly ITransferFrequencyDateCalculatorFactory transferFrequencyDateCalculatorFactory;
        readonly ITransferDirectionGenerator transferDirectionGenerator;

        public ProjectionTransferGenerator(
                        IBankAccountRepository bankAccountRepository,
                        ITransferFrequencyDateCalculatorFactory transferFrequencyDateCalculatorFactory,
                        ITransferDirectionGenerator transferDirectionGenerator
                        )
        {
            this.bankAccountRepository = bankAccountRepository;
            this.transferFrequencyDateCalculatorFactory = transferFrequencyDateCalculatorFactory;
            this.transferDirectionGenerator = transferDirectionGenerator;
        }



        public List<CashflowProjectionTransfer> GenerateProjectionTransfers(
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

            List<TransferDirection> transferDirections = transferDirectionGenerator.GetTransferDirections(bankAccounts);

            // extrapolate date range.
            // loop each transfer
            foreach (var td in transferDirections)
            {
                ITransferFrequencyDateCalculator transferFrequencyDateCalculator;

                Transfer t = td.Transfer;

                transferFrequencyDateCalculator = this.transferFrequencyDateCalculatorFactory.Create(t);

                // loop compatible date range
                DateTime d = t.StartDate;// < startDate ? t.StartDate : startDate;
                while (d <= endDate && (t.EndDate == null || d <= t.EndDate))
                {
                    if(d>= startDate && d<=endDate)
                        cpts.Add(new CashflowProjectionTransfer() { Date = d, TransferDirection = td });

                    d = transferFrequencyDateCalculator.CalculateNextDate(t, d);
                }
            }

            return cpts;
        }


        //private List<TransferDirection> GetTransferDirections(List<BankAccount> bankAccounts)
        //{
        //    var transferDirections = (from t in this.transferRepository.ReadList()
        //                              let outBound = t.FromBankAccount != null && bankAccounts.Count(a => a.BankAccountId == t.FromBankAccount.BankAccountId) > 0
        //                              let inBound = t.ToBankAccount != null && bankAccounts.Count(a => a.BankAccountId == t.ToBankAccount.BankAccountId) > 0
        //                              where inBound ^ outBound  // xor == exclude internal transfers
        //                                && t.IsEnabled
        //                              select new TransferDirection()
        //                              {
        //                                  Transfer = t,
        //                                  IsOutbound = outBound,
        //                                  IsInbound = inBound
        //                              }
        //        ).ToList();

        //    return transferDirections;
        //}

    }
}
