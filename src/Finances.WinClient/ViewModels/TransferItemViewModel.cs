using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Core.Wpf;

namespace Finances.WinClient.ViewModels
{
    public class TransferItemViewModel : ItemViewModelBase
    {
        Transfer entity;


        public TransferItemViewModel(Transfer entity)
        {
            this.entity = entity;
        }


        public Transfer Entity
        {
            get { return entity; }
            set 
            { 
                entity = value;
                fromBankAccount = null;
                toBankAccount = null;
                NotifyAllPropertiesChanged();
            }
        }


        public int TransferId
        {
            get
            {
                return entity.TransferId;
            }
        }

        public string Name
        {
            get
            {
                return entity.Name;
            }
        }


        BankAccountItemViewModel fromBankAccount;
        public BankAccountItemViewModel FromBankAccount
        {
            get
            {
                if (entity.FromBankAccount != null && fromBankAccount == null)
                    fromBankAccount = new BankAccountItemViewModel(entity.FromBankAccount);

                return fromBankAccount;
            }
        }

        BankAccountItemViewModel toBankAccount;
        public BankAccountItemViewModel ToBankAccount
        {
            get
            {
                if (entity.ToBankAccount != null && toBankAccount == null)
                    toBankAccount = new BankAccountItemViewModel(entity.ToBankAccount);

                return toBankAccount;
            }
        }

        public decimal Amount
        {
            get
            {
                return entity.Amount;
            }
        }

        public decimal AmountTolerence
        {
            get
            {
                return entity.AmountTolerence;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return entity.Schedule.StartDate;
            }
        }

        public DateTime? EndDate
        {
            get
            {
                return entity.Schedule.EndDate;
            }
        }

        public string Frequency
        {
            get
            {
                return entity.Schedule.Frequency;
            }
        }

        public int FrequencyEvery
        {
            get
            {
                return entity.Schedule.FrequencyEvery;
            }
        }

        public string ScheduleDescription
        {
            get
            {
                return entity.Schedule.GetDescription();
            }
        }


        public bool IsEnabled
        {
            get
            {
                return entity.IsEnabled;   
            }
        }

        public TransferCategory Category
        {
            get
            {
                return entity.Category;
            }
        }


    }
}
