using Finances.Core.Interfaces;
using Finances.Core.Wpf;
using Finances.Core.Entities;

namespace Finances.WinClient.ViewModels
{
    public class CashflowBankAccountItemViewModel : TreeViewItemViewModelBase
    {
        CashflowBankAccount entity;

        public CashflowBankAccountItemViewModel(CashflowBankAccount entity)
        {
            this.entity = entity;
        }

        public int CashflowBankAccountId
        {
            get { return entity.CashflowBankAccountId; }
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



        public override string ToString()
        {
            return BankAccount == null ? "?" : (BankAccount.Bank == null ? "?" : BankAccount.Bank.Name) + "-" + BankAccount.AccountName;
        }


    }
}
