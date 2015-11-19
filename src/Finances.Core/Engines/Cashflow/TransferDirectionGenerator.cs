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
    public class TransferDirectionGenerator : ITransferDirectionGenerator
    {
        readonly ITransferRepository transferRepository;

        public TransferDirectionGenerator(
                        ITransferRepository transferRepository
                        )
        {
            this.transferRepository = transferRepository;

        }

        public List<TransferDirection> GetTransferDirections(List<BankAccount> bankAccounts)
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

    }
}
