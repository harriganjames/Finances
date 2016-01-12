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


        public bool IsInbound
        {
            get
            {
                return (FromBankAccount == null || FromBankAccount.BankAccountId == -1);
            }
        }

        public bool IsOutound
        {
            get
            {
                return (ToBankAccount == null || ToBankAccount.BankAccountId == -1);
            }
        }


        public bool IsTransfer
        {
            get
            {
                return !this.IsInbound && !this.IsOutound;
            }
        }


        public string DirectionName
        {
            get
            {
                string direction = "Unknown";

                if (this.IsInbound)
                {
                    direction = "Inbound";
                }
                else if (this.IsOutound)
                {
                    direction = "Outbound";
                }
                else
                {
                    direction = "Transfer";
                }

                return direction;
            }
        }


        //#region IScheduleEntity
        //public System.DateTime StartDate { get; set; }
        //public Nullable<System.DateTime> EndDate { get; set; }
        //public string Frequency { get; set; }
        //public int FrequencyEvery { get; set; }
        //#endregion
    }
}
