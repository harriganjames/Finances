using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Finances.Core.Interfaces;
using Finances.Core.Wpf;


namespace Finances.WinClient.ViewModels
{
    public class CashflowItemViewModel : TreeViewItemViewModelBase
    {

        int cashflowId;
        public int CashflowId
        {
            get { return cashflowId; }
            set { cashflowId = value; NotifyPropertyChanged(); }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; NotifyPropertyChanged(); }
        }

        decimal openingBalance;
        public decimal OpeningBalance
        {
            get { return openingBalance; }
            set { openingBalance = value; NotifyPropertyChanged(); }
        }

        DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; NotifyPropertyChanged(); }
        }

        ObservableCollection<CashflowBankAccountItemViewModel> cashflowBankAccounts;
        public ObservableCollection<CashflowBankAccountItemViewModel> CashflowBankAccounts
        {
            get 
            {
                if (cashflowBankAccounts == null)
                    cashflowBankAccounts = new ObservableCollection<CashflowBankAccountItemViewModel>();
                return cashflowBankAccounts; 
            }
            set { cashflowBankAccounts = value; NotifyPropertyChanged(); }
        }




        public override string ToString()
        {
            return Name;
        }


    }
}
