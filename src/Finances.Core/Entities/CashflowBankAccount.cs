using System;

namespace Finances.Core.Entities
{
    public class CashflowBankAccount
    {
        public int CashflowBankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }
        public DateTime RecordCreatedDateTime { get; set; }
    }
}
