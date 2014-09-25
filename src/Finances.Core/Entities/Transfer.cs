using System;

namespace Finances.Core.Entities
{
    public class Transfer
    {
        public virtual int TransferId { get; set; }
        public virtual string Name { get; set; }
        public virtual BankAccount FromBankAccount { get; set; }
        public virtual BankAccount ToBankAccount { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual decimal AmountTolerence { get; set; }
        public virtual System.DateTime StartDate { get; set; }
        public virtual Nullable<System.DateTime> EndDate { get; set; }
        public virtual string Frequency { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual System.DateTime RecordCreatedDateTime { get; set; }
        public virtual System.DateTime RecordUpdatedDateTime { get; set; }
    }
}
