using System;
using Finances.Core.Interfaces;
using Finances.Core.Factories;

namespace Finances.Core.Entities
{
    public class Transfer : Entity//, IScheduleEntity
    {
        IScheduleFactory scheduleFactory;

        public Transfer(IScheduleFactory scheduleFactory)
        {
            this.scheduleFactory = scheduleFactory;

            //FromBankAccount = new BankAccount();
            //ToBankAccount = new BankAccount();
            this.Schedule = this.scheduleFactory.Create();
            //this.FrequencyEvery = 1;
        }

        public int TransferId { get; set; }
        public string Name { get; set; }
        public BankAccount FromBankAccount { get; set; }
        public BankAccount ToBankAccount { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountTolerence { get; set; }
        public bool IsEnabled { get; set; }
        public TransferCategory Category { get; set; }
        public System.DateTime RecordCreatedDateTime { get; set; }
        public System.DateTime RecordUpdatedDateTime { get; set; }

        public Schedule Schedule { get; set; }

        //#region IScheduleEntity
        //public System.DateTime StartDate { get; set; }
        //public Nullable<System.DateTime> EndDate { get; set; }
        //public string Frequency { get; set; }
        //public int FrequencyEvery { get; set; }
        //#endregion
    }
}
