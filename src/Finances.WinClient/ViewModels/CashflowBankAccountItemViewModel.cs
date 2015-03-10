using Finances.Core.Interfaces;
using Finances.Core.Wpf;


namespace Finances.WinClient.ViewModels
{
    public class CashflowBankAccountItemViewModel : TreeViewItemViewModelBase
    {

        int cashflowBankAccountId;
        public int CashflowBankAccountId
        {
            get { return cashflowBankAccountId; }
            set { cashflowBankAccountId = value; NotifyPropertyChanged(); }
        }

        private BankAccountItemViewModel bankAccount;
        public BankAccountItemViewModel BankAccount
        {
            get 
            {
                if (bankAccount == null)
                    bankAccount = new BankAccountItemViewModel();
                return bankAccount; 
            }
            set { bankAccount = value; NotifyPropertyChanged(); }
        }




        public override string ToString()
        {
            return BankAccount == null ? "?" : (BankAccount.Bank == null ? "?" : BankAccount.Bank.Name) + "-" + BankAccount.AccountName;
        }


    }
}
