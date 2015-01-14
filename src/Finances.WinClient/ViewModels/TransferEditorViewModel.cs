using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

using Finances.Core.Wpf;
using Finances.WinClient.DomainServices;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using System.Threading.Tasks;


namespace Finances.WinClient.ViewModels
{
    public interface ITransferEditorViewModel : IEditorViewModelBase, IEntityMapper<Transfer>
    {
        void InitializeForAddEdit(bool AddEdit);

        int TransferId { get; set; }
        string Name { get; set; }
        IBankAccountItemViewModel FromBankAccount { get; set; }
        IBankAccountItemViewModel ToBankAccount { get; set; }
        InputDecimal Amount { get; set; }
        decimal AmountTolerence { get; set; }
        DateTime? StartDate { get; set; }
        Nullable<DateTime> EndDate { get; set; }
        string Frequency { get; set; }
        bool IsEnabled { get; set; }
    }

    public class TransferEditorViewModel : EditorViewModelBase, ITransferEditorViewModel
    {
        bool delayValidation = false; // must be false until change logic around IsValid
        ObservableCollection<IBankAccountItemViewModel> bankAccounts;
        List<DataIdName> existingTransfers;


        readonly ITransferService transferService;
        readonly IBankAccountService bankAccountService;


        public TransferEditorViewModel(ITransferService transferService,
            IBankAccountService bankAccountService)
        {
            this.transferService = transferService;
            this.bankAccountService = bankAccountService;

            this.Amount.PropertyChanged += (s,e) =>
                {
                    base.Validate();
                };
        }




        #region Publics


        public void InitializeForAddEdit(bool addMode)
        {
            base.ValidationHelper.Enabled = !delayValidation;
            base.ValidationHelper.Reset();

            LoadBankAccountList();
            LoadExistingTransfers();

            //if (base.ValidationHelper.Enabled)
            //    base.Validate();

            if (addMode)
            {
                //this.FromBankAccount = elsewhere;
                //this.ToBankAccount = elsewhere;
                this.StartDate = DateTime.Now.Date;
                this.IsEnabled = true;

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


        public ObservableCollection<IBankAccountItemViewModel> BankAccounts
        {
            get
            {
                return this.bankAccounts;
            }
        }

        ObservableCollection<string> frequencies;
        public ObservableCollection<string> Frequencies
        {
            get
            {
                if (frequencies == null)
                {
                    frequencies = new ObservableCollection<string>() { "Monthly" };
                }
                return frequencies;
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
                NotifyPropertyChangedAndValidate();
            }
        }


        IBankAccountItemViewModel fromBankAccount;
        public IBankAccountItemViewModel FromBankAccount
        {
            get
            {
                if (fromBankAccount == null)
                    fromBankAccount = BankAccountItemViewModel.Elsewhere;

                if (this.BankAccounts != null && !this.BankAccounts.Contains(fromBankAccount))
                {
                    IBankAccountItemViewModel find = this.BankAccounts.FirstOrDefault(a => a!=null && a.BankAccountId == fromBankAccount.BankAccountId);
                    if (find != null)
                        fromBankAccount = find;
                }

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
                if (toBankAccount == null)
                    toBankAccount = BankAccountItemViewModel.Elsewhere;

                if (this.BankAccounts != null && !this.BankAccounts.Contains(toBankAccount))
                {
                    IBankAccountItemViewModel find = this.BankAccounts.FirstOrDefault(a => a!=null && a.BankAccountId == toBankAccount.BankAccountId);
                    if (find != null)
                        toBankAccount = find;
                }


                return toBankAccount;
            }
            set
            {
                toBankAccount = value;
                NotifyPropertyChangedAndValidate();
            }
        }


        InputDecimal amount = new InputDecimal() { FormatString = "n2", Mandatory = true };
        public InputDecimal Amount
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


        //decimal? amount;
        //public decimal? Amount
        //{
        //    get
        //    {
        //        return amount;
        //    }
        //    set
        //    {
        //        amount = value;
        //        NotifyPropertyChangedAndValidate();
        //    }
        //}
        //string amountString;
        //public string AmountString
        //{
        //    get
        //    {
        //        if (amount.HasValue)
        //            return amount.Value.ToString("n2");
        //        else
        //            return amountString; // String.Empty;
        //    }
        //    set
        //    {
        //        decimal v;
        //        if (Decimal.TryParse(value, out v))
        //            amount = v;
        //        else
        //            amount = null;
        //        amountString = value;
        //        NotifyPropertyChangedAndValidate();
        //    }
        //}

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



        DateTime? startDate;
        [Required(ErrorMessage = "Start Date is mandatory")]
        public DateTime? StartDate
        {
            get { return startDate.HasValue ? startDate.Value.Date : startDate; }
            set { startDate = value.HasValue ? value.Value.Date : value; NotifyPropertyChangedAndValidate(); }
        }

        DateTime? endDate;
        public DateTime? EndDate
        {
            get { return endDate.HasValue ? endDate.Value.Date : endDate; }
            set { endDate = value.HasValue ? value.Value.Date : value; NotifyPropertyChangedAndValidate(); }
        }

        string frequency;
        public string Frequency
        {
            get
            {
                if (frequency == null && this.Frequencies != null && this.Frequencies.Count() > 0)
                    frequency = this.Frequencies[0];
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
            bankAccounts = new ObservableCollection<IBankAccountItemViewModel>();
            BankAccounts.Add(BankAccountItemViewModel.Elsewhere);
            //BankAccounts.Add(null);
            bankAccountService.ReadList().ForEach(vm => BankAccounts.Add(vm));
        }

        private void LoadExistingTransfers()
        {
            Task.Factory.StartNew(() =>
                {
                    existingTransfers = transferService.ReadListDataIdName();
                });

            //existingTransfers = transferService.ReadListDataIdName();



            //existingTransfers = transferService.ReadListDataIdName();
        }

        #endregion

        #region Validation

        // Use annotations and/or ValidateDate() method
        // Add errors to ValidationHelper:
        // base.ValidationHelper.AddValidationMessage("Bank Name already exists", "Name");

        protected override void ValidateData()
        {
            //Transfer exists
            if (existingTransfers != null)
            {
                string uniqueName = String.Format("{0}", this.Name);
                if (existingTransfers.Exists(n => n.Name.Equals(uniqueName, StringComparison.CurrentCultureIgnoreCase) && n.Id != this.TransferId))
                {
                    base.ValidationHelper.AddValidationMessage("Transfer already exists", "Name");
                }
            }


            if (this.FromBankAccount == this.ToBankAccount)
            {
                base.ValidationHelper.AddValidationMessage("From and To Accounts must be different");
            }

            if (!amount.IsNumeric)
            {
                // probbaly needs IErrorInfo on BackingDecimal ??
                base.ValidationHelper.AddValidationMessage("Amount is invalid", "Amount.Input");
            }
            else if (!amount.HasValue)
            {
                base.ValidationHelper.AddValidationMessage("Amount is mandatory", "Amount.Input");
            }
            else if (amount.Value<=0)
            {
                base.ValidationHelper.AddValidationMessage("Amount must be > 0", "Amount.Input");
            }



            // Bank
            //if (this.bankAccountList != null)
            //{
            //    if (!this.BankAccountList.Contains(this.FromBankAccount))
            //        base.ValidationHelper.AddValidationMessage("Invalid Bank", "Bank");
            //}
        }

        #endregion


        #region IEntityMapper

        public void MapIn(Transfer entity)
        {
            this.TransferId = entity.TransferId;
            this.Name = entity.Name;

            if (entity.FromBankAccount == null)
                this.FromBankAccount = BankAccountItemViewModel.Elsewhere;
            else
            {
                this.FromBankAccount = new BankAccountItemViewModel();
                this.FromBankAccount.MapIn(entity.FromBankAccount);
            }

            if (entity.ToBankAccount == null)
                this.ToBankAccount = BankAccountItemViewModel.Elsewhere;
            else
            {
                this.ToBankAccount = new BankAccountItemViewModel();
                this.ToBankAccount.MapIn(entity.ToBankAccount); ;
            }
            
            this.Amount.Value = entity.Amount;
            this.AmountTolerence = entity.AmountTolerence;
            this.StartDate = entity.StartDate;
            this.EndDate = entity.EndDate;
            this.Frequency = entity.Frequency;
            this.IsEnabled = entity.IsEnabled;
        }

        public void MapOut(Transfer entity)
        {
            entity.TransferId = this.TransferId;
            entity.Name = this.Name;

            if (this.FromBankAccount.BankAccountId == BankAccountItemViewModel.Elsewhere.BankAccountId)
                entity.FromBankAccount = null;
            else
                this.FromBankAccount.MapOut(entity.FromBankAccount);

            if (this.ToBankAccount.BankAccountId == BankAccountItemViewModel.Elsewhere.BankAccountId)
                entity.ToBankAccount = null;
            else
                this.ToBankAccount.MapOut(entity.ToBankAccount);
            
            entity.Amount = this.Amount.Value;
            entity.AmountTolerence = this.AmountTolerence;
            entity.StartDate = this.StartDate.Value;
            entity.EndDate = this.EndDate;
            entity.Frequency = this.Frequency;
            entity.IsEnabled = this.IsEnabled;
        }

        #endregion
    }
}
