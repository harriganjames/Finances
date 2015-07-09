using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Core.Wpf;


namespace Finances.WinClient.ViewModels
{
    public class CashflowItemViewModel : TreeViewItemViewModelBase
    {
        Cashflow entity;

        public CashflowItemViewModel(Cashflow entity)
        {
            this.entity = entity;
        }

        public Cashflow Entity
        {
            get { return entity; }
            set 
            { 
                entity = value;
                NotifyAllPropertiesChanged();
            }
        }


        public int CashflowId
        {
            get { return entity.CashflowId; }
        }

        public string Name
        {
            get { return entity.Name; }
        }

        public decimal OpeningBalance
        {
            get { return entity.OpeningBalance; }
        }

        public DateTime StartDate
        {
            get { return entity.StartDate; }
        }


        public string Accounts
        {
            get
            {
                if (entity.CashflowBankAccounts.Count == 0)
                    return "All";
                else
                    return entity.CashflowBankAccounts.Count.ToString();
            }
        }

        ObservableCollection<CashflowBankAccountItemViewModel> cashflowBankAccounts;
        public ObservableCollection<CashflowBankAccountItemViewModel> CashflowBankAccounts
        {
            get 
            {
                if (cashflowBankAccounts == null)
                {
                    cashflowBankAccounts = new ObservableCollection<CashflowBankAccountItemViewModel>();
                    foreach (var cba in entity.CashflowBankAccounts)
                    {
                        cashflowBankAccounts.Add(new CashflowBankAccountItemViewModel(cba));
                    }
                }
                return cashflowBankAccounts; 
            }
        }




        public override string ToString()
        {
            return Name;
        }


    }
}
