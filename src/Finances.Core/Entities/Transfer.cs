using System;

namespace Finances.Core.Entities
{
    public class Transfer : Entity
    {
        public Transfer()
        {
            //FromBankAccount = new BankAccount();
            //ToBankAccount = new BankAccount();
            this.FrequencyDays = 1;
        }

        public int TransferId { get; set; }
        public string Name { get; set; }
        public BankAccount FromBankAccount { get; set; }
        public BankAccount ToBankAccount { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountTolerence { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Frequency { get; set; }
        public int FrequencyDays { get; set; }
        public bool IsEnabled { get; set; }
        public TransferCategory Category { get; set; }
        public System.DateTime RecordCreatedDateTime { get; set; }
        public System.DateTime RecordUpdatedDateTime { get; set; }
    }
}
