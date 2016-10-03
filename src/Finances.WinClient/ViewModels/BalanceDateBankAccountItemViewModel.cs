using Finances.Core.Interfaces;
using Finances.Core.Wpf;
using Finances.Core.Entities;

namespace Finances.WinClient.ViewModels
{
    public class BalanceDateBankAccountItemViewModel : ValidationViewModelBase
    {
        BalanceDateBankAccount entity;

        public BalanceDateBankAccountItemViewModel(BalanceDateBankAccount entity)//, bool validationEnabled = false)
        {
            this.entity = entity;
            if (this.entity.BalanceDateBankAccountId > 0) balanceAmount = this.entity.BalanceAmount;
            //base.ValidationHelper.Enabled = validationEnabled;
            isBalanceAmountValid = true;
        }

        public BalanceDateBankAccount Entity
        {
            get
            {
                return entity;
            }
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

        decimal? balanceAmount;
        public decimal? BalanceAmount
        {
            get
            {
                return balanceAmount;
            }
            set
            {
                if (balanceAmount != value)
                {
                    balanceAmount = value;
                    entity.BalanceAmount = balanceAmount.GetValueOrDefault();
                    NotifyPropertyChangedAndValidate();
                }
            }
        }

        bool isBalanceAmountValid;
        public bool IsBalanceAmountValid
        {
            get
            {
                return isBalanceAmountValid;
            }
            set
            {
                isBalanceAmountValid = value;
                NotifyPropertyChangedAndValidate();
            }
        }

        public override string ToString()
        {
            return BankAccount == null ? "?" : (BankAccount.Bank == null ? "?" : BankAccount.Bank.Name) + "-" + BankAccount.AccountName;
        }


        #region Validation

        // Use annotations and/or ValidateData() method
        // Add errors to ValidationHelper:
        // base.ValidationHelper.AddValidationMessage("Bank Name already exists", "Name");

        protected override void ValidateData()
        {
            if (!IsBalanceAmountValid)
            {
                base.ValidationHelper.AddValidationMessage("Balance Amount is invalid", "IsBalanceAmountValid");
            }
        }

        #endregion

    }
}
