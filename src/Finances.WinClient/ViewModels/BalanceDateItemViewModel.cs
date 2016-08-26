using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Core.Wpf;
using System.Collections.ObjectModel;

namespace Finances.WinClient.ViewModels
{
    public class BalanceDateItemViewModel : ItemViewModelBase
    {
        BalanceDate entity;

        public BalanceDateItemViewModel(BalanceDate entity)
        {
            this.entity = entity;
        }


        public BalanceDate Entity
        {
            get { return entity; }
            set
            {
                entity = value;
                balanceDateBankAccounts = null;
                //bank = new BankItemViewModel(entity.Bank);
                NotifyAllPropertiesChanged();
            }
        }


        public int BalanceDateId
        {
            get { return entity.BalanceDateId; }
        }

        public DateTime DateOfBalance
        {
            get { return entity.DateOfBalance; }
        }


        ObservableCollection<BalanceDateBankAccountItemViewModel> balanceDateBankAccounts;
        public ObservableCollection<BalanceDateBankAccountItemViewModel> BalanceDateBankAccounts
        {
            get
            {
                if (balanceDateBankAccounts == null)
                {
                    balanceDateBankAccounts = new ObservableCollection<BalanceDateBankAccountItemViewModel>();
                    foreach (var e in entity.BalanceDateBankAccounts)
                    {
                        balanceDateBankAccounts.Add(new BalanceDateBankAccountItemViewModel(e));
                    }
                }
                return balanceDateBankAccounts;
            }
        }


    }
}
