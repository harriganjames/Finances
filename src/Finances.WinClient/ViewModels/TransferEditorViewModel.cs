using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

using Finances.Core.Wpf;
using Finances.WinClient.DomainServices;
using Finances.Core.Entities;


namespace Finances.WinClient.ViewModels
{
    public interface ITransferEditorViewModel : IEditorViewModelBase
    {
        void InitializeForAddEdit(bool AddEdit);

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
    }

    public class TransferEditorViewModel : EditorViewModelBase, ITransferEditorViewModel
    {
        bool delayValidation = false; // must be false until change logic around IsValid
        ObservableCollection<IBankAccountItemViewModel> bankAccountList;
        List<DataIdName> existingTransfers;


        readonly ITransferService transferService;
        //readonly IBankService bankService;


        public TransferEditorViewModel(ITransferService transferService)
        //IBankService bankService)
        {
            this.transferService = transferService;
            //this.bankService = bankService;
        }



        #region Publics


        public void InitializeForAddEdit(bool addMode)
        {
            base.ValidationHelper.Enabled = !delayValidation;
            base.ValidationHelper.Reset();

            LoadBankAccountList();
            LoadExistingTransfers();

            if (base.ValidationHelper.Enabled)
                base.Validate();

            if (addMode)
            {
                //this.BankAccountId = 0;
                //this.AccountName = "";
                //this.Bank = null;
                //this.AccountNumber = "";
                //this.SortCode = "";
                //this.AccountOwner = "";
                //this.OpenedDate = DateTime.Now.Date;
                //this.ClosedDate = null;
                //this.InitialRate = 0;
                //this.LoginId = "";
                //this.LoginUrl = "";
                //this.MilestoneDate = null;
                //this.MilestoneNotes = "";
                //this.Notes = "";
                //this.PasswordHint = "";
                //this.PaysTaxableInterest = false;

            }

        }

        public string DialogTitle
        {
            get
            {
                return this.TransferId == 0 ? String.Format("Add new Transfer") : String.Format("Edit Transfer {0}", this.Name);
            }
        }


        public ObservableCollection<IBankAccountItemViewModel> BankAccountList
        {
            get
            {
                return this.bankAccountList;
            }
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
                NotifyPropertyChangedAndValidate();
            }
        }

        string name;
        [Required(ErrorMessage = "Transfer Name is mandatory")]
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
                return fromBankAccount;
            }
            set
            {
                fromBankAccount = value;
                NotifyPropertyChangedAndValidate();
            }
        }

        IBankAccountItemViewModel toBankAccount;
        public IBankAccountItemViewModel ToBankAccount
        {
            get
            {
                return toBankAccount;
            }
            set
            {
                toBankAccount = value;
                NotifyPropertyChangedAndValidate();
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
                NotifyPropertyChangedAndValidate();
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
                NotifyPropertyChangedAndValidate();
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
                NotifyPropertyChangedAndValidate();
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
                NotifyPropertyChangedAndValidate();
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
                NotifyPropertyChangedAndValidate();
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
                NotifyPropertyChangedAndValidate();
            }
        }

        #endregion

        #region Privates

        private void LoadBankAccountList()
        {
            bankAccountList = new ObservableCollection<IBankAccountItemViewModel>();
//            transferService.ReadAccountList().ForEach(vm => bankAccountList.Add(vm));
        }

        private void LoadExistingTransfers()
        {
            existingTransfers = transferService.ReadListDataIdName();
        }

        #endregion

        #region Validation

        // Use annotations and/or ValidateDate() method
        // Add errors to ValidationHelper:
        // base.ValidationHelper.AddValidationMessage("Bank Name already exists", "Name");

        protected override void ValidateData()
        {
            // Bank Account exists
            //if (existingTransfers != null)
            //{
            //    string uniqueName = String.Format("{0} ({1})", this.AccountName, this.Bank == null ? "" : this.Bank.Name);
            //    if (existingTransfers.Exists(n => n.Name.Equals(uniqueName, StringComparison.CurrentCultureIgnoreCase) && n.Id != this.BankAccountId))
            //    {
            //        base.ValidationHelper.AddValidationMessage("Bank Account already exists", "AccountName");
            //    }
            //}


            // Bank
            //if (this.bankAccountList != null)
            //{
            //    if (!this.BankAccountList.Contains(this.FromBankAccount))
            //        base.ValidationHelper.AddValidationMessage("Invalid Bank", "Bank");
            //}
        }

        #endregion

    }
}
