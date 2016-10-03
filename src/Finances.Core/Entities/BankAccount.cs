using System;
using System.Collections.Generic;

namespace Finances.Core.Entities
{
    public class BankAccount : IEqualityComparer<BankAccount>
    {
        public BankAccount()
        {
            this.Bank = new Bank();
        }

        public virtual int BankAccountId { get; set; }
        public virtual string Name { get; set; }
        //public virtual int BankId { get; set; }
        public virtual string SortCode { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual string AccountOwner { get; set; }
        public virtual bool PaysTaxableInterest { get; set; }
        public virtual string LoginURL { get; set; }
        public virtual string LoginID { get; set; }
        public virtual string PasswordHint { get; set; }
        public virtual System.DateTime OpenedDate { get; set; }
        public virtual Nullable<System.DateTime> ClosedDate { get; set; }
        public virtual Nullable<decimal> InitialRate { get; set; }
        public virtual Nullable<System.DateTime> MilestoneDate { get; set; }
        public virtual string MilestoneNotes { get; set; }
        public virtual string Notes { get; set; }
        public virtual System.DateTime RecordCreatedDateTime { get; set; }
        public virtual System.DateTime RecordUpdatedDateTime { get; set; }

        public virtual Bank Bank { get; set; }

        public bool Equals(BankAccount x, BankAccount y)
        {
            return x.BankAccountId == y.BankAccountId;
        }

        public int GetHashCode(BankAccount obj)
        {
            return obj.GetHashCode();
        }


    }
}
