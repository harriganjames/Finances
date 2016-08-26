using Finances.Core.Interfaces;
using Finances.Core.Wpf;
using Finances.Core.Entities;

namespace Finances.WinClient.ViewModels
{
    public class BalanceDateBankAccountItemViewModel : TreeViewItemViewModelBase
    {
        BalanceDateBankAccount entity;

        public BalanceDateBankAccountItemViewModel(BalanceDateBankAccount entity)
        {
            this.entity = entity;
        }

        public int BalanceDateBankAccountId
        {
            get { return entity.BalanceDateBankAccountId; }
        }

        private BankAccountItemViewModel bankAccount;
        public BankAccountItemViewModel BankAccount
        {
            get 
            {
                if (bankAccount == null)
                    bankAccount = new BankAccountItemViewModel(entity.BankAccount);
                return bankAccount; 
            }
        }

       public decimal BalanceAmount
        {
            get
            {
                return entity.BalanceAmount;
            }
        }


        public override string ToString()
        {
            return BankAccount == null ? "?" : (BankAccount.Bank == null ? "?" : BankAccount.Bank.Name) + "-" + BankAccount.AccountName;
        }


    }
}
