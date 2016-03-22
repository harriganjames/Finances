using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Core.ValueObjects;


namespace Finances.Core.Engines.Cashflow
{
    public class CashflowProjectionTransferGenerator : ICashflowProjectionTransferGenerator
    {
        readonly IBankAccountRepository bankAccountRepository;
        readonly ITransferDirectionGenerator transferDirectionGenerator;

        public CashflowProjectionTransferGenerator(
                        IBankAccountRepository bankAccountRepository,
                        ITransferDirectionGenerator transferDirectionGenerator
                        )
        {
            this.bankAccountRepository = bankAccountRepository;
            this.transferDirectionGenerator = transferDirectionGenerator;
        }



        public async Task<List<CashflowProjectionTransfer>> GenerateCashflowProjectionTransfersAsync(
                                List<CashflowBankAccount> accounts,
                                DateTime startDate,
                                DateTime endDate)
        {
            var task = Task.Factory.StartNew(() =>
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
                    Transfer t = td.Transfer;

                    // loop compatible date range
                    DateTime d = t.Schedule.StartDate;
                    while (d <= endDate && (t.Schedule.EndDate == null || d <= t.Schedule.EndDate))
                    {
                        if (d >= startDate && d <= endDate)
                            cpts.Add(new CashflowProjectionTransfer() { Date = d, TransferDirection = td });

                        d = t.Schedule.CalculateNextDate(d);
                    }
                }

                return cpts;

            });

            return await task;            
        }
    }
}
