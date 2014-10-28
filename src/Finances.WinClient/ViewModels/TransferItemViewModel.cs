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
    public interface ITransferItemViewModel : IItemViewModelBase, IEntityMapper<Transfer>
    {
        int TransferId { get; set; }
        string Name { get; set; }
        IBankAccountItemViewModel FromBankAccount { get; set; }
        IBankAccountItemViewModel ToBankAccount { get; set; }
        decimal Amount { get; set; }
        decimal AmountTolerence { get; set; }
        DateTime StartDate { get; set; }
        Nullable<DateTime> EndDate { get; set; }
        string Frequency { get; set; }
        bool IsEnabled { get; set; }
        //ITransferItemViewModel MapIn(Transfer from);
    }

    public class TransferItemViewModel : ItemViewModelBase, ITransferItemViewModel
    {

        public TransferItemViewModel()
        {
        }



        int transferId;
        public int TransferId
        {
            get
            {
                return transferId;
            }
            set
            {
                transferId = value;
                NotifyPropertyChanged();
            }
        }

        string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }


        IBankAccountItemViewModel fromBankAccount;
        public IBankAccountItemViewModel FromBankAccount
        {
            get
            {
                if (fromBankAccount == null)
                    fromBankAccount = new BankAccountItemViewModel();
                return fromBankAccount;
            }
            set
            {
                fromBankAccount = value;
                NotifyPropertyChanged();
            }
        }

        IBankAccountItemViewModel toBankAccount;
        public IBankAccountItemViewModel ToBankAccount
        {
            get
            {
                if (toBankAccount == null)
                    toBankAccount = new BankAccountItemViewModel();
                return toBankAccount;
            }
            set
            {
                toBankAccount = value;
                NotifyPropertyChanged();
            }
        }

        decimal amount;
        public decimal Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
                NotifyPropertyChanged();
            }
        }

        decimal amountTolerence;
        public decimal AmountTolerence
        {
            get
            {
                return amountTolerence;
            }
            set
            {
                amountTolerence = value;
                NotifyPropertyChanged();
            }
        }

        DateTime startDate;
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
                NotifyPropertyChanged();
            }
        }

        DateTime? endDate;
        public DateTime? EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                endDate = value;
                NotifyPropertyChanged();
            }
        }

        string frequency;
        public string Frequency
        {
            get
            {
                return frequency;
            }
            set
            {
                frequency = value;
                NotifyPropertyChanged();
            }
        }

        bool isEnabled;
        public bool IsEnabled
        {
            get
            {
                return isEnabled;   
            }
            set
            {
                isEnabled = value;
                NotifyPropertyChanged();
            }
        }



        //public ITransferItemViewModel MapIn(Transfer from)
        //{
        //    this.TransferId = from.TransferId;
        //    this.Name = from.Name;
        //    this.FromBankAccount = this.FromBankAccount.MapIn(from.FromBankAccount);
        //    this.ToBankAccount = this.ToBankAccount.MapIn(from.ToBankAccount); ;
        //    this.Amount = from.Amount;
        //    this.AmountTolerence = from.AmountTolerence;
        //    this.StartDate = from.StartDate;
        //    this.EndDate = from.EndDate;
        //    this.Frequency = from.Frequency;
        //    this.IsEnabled = from.IsEnabled;
        //    return this;
        //}


        #region IEntityMapper

        public void MapIn(Transfer entity)
        {
            this.TransferId = entity.TransferId;
            this.Name = entity.Name;
            this.FromBankAccount.MapIn(entity.FromBankAccount);
            this.ToBankAccount.MapIn(entity.ToBankAccount); ;
            this.Amount = entity.Amount;
            this.AmountTolerence = entity.AmountTolerence;
            this.StartDate = entity.StartDate;
            this.EndDate = entity.EndDate;
            this.Frequency = entity.Frequency;
            this.IsEnabled = entity.IsEnabled;
        }

        public void MapOut(Transfer entity)
        {
            entity.TransferId = this.TransferId;
        }

        #endregion

        //ITransferItemViewModel ITransferItemViewModel.MapIn(Transfer from)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
