using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

using Finances.Core.Wpf;
using Finances.WinClient.DomainServices;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using AutoMapper;
using Finances.WinClient.Factories;
using System.Diagnostics;


namespace Finances.WinClient.ViewModels
{
    //public interface BankAccountEditorViewModel : IEditorViewModelBase, IEntityMapper<BankAccount>
    //{
    //    void InitializeForAddEdit(bool AddEdit);

    //    string AccountName { get; set; }
    //    string AccountNumber { get; set; }
    //    string AccountOwner { get; set; }
    //    BankItemViewModel Bank { get; set; }
    //    int BankAccountId { get; set; }
    //    int BankId { get; set; }
    //    DateTime? OpenedDate { get; set; }
    //    DateTime? ClosedDate { get; set; }
    //    decimal? InitialRate { get; set; }
    //    string LoginId { get; set; }
    //    string LoginUrl { get; set; }
    //    DateTime? MilestoneDate { get; set; }
    //    string MilestoneNotes { get; set; }
    //    string Notes { get; set; }
    //    string PasswordHint { get; set; }
    //    bool PaysTaxableInterest { get; set; }
    //    string SortCode { get; set; }
    //}

    public class BankAccountEditorViewModel : EditorViewModelBase//, BankAccountEditorViewModel
    {
        bool delayValidation = false; // must be false until change logic around IsValid
        ObservableCollection<BankItemViewModel> bankList;
        List<DataIdName> existingBankAccounts;

        readonly IBankAccountRepository bankAccountRepository;
        readonly IBankRepository bankRepository;
        readonly IMappingEngine mapper;
        readonly IDialogService dialogService;
        readonly IBankEditorViewModelFactory bankEditorViewModelFactory;


        public BankAccountEditorViewModel(
                        IBankAccountRepository bankAccountRepository,
                        IBankRepository bankRepository,
                        IMappingEngine mapper,
                        IDialogService dialogService,
                        IBankEditorViewModelFactory bankEditorViewModelFactory
                        )
        {
            this.bankRepository = bankRepository;
            this.bankAccountRepository = bankAccountRepository;
            this.mapper = mapper;
            this.dialogService = dialogService;
            this.bankEditorViewModelFactory = bankEditorViewModelFactory;

            NewBankCommand = base.AddNewCommand(new ActionCommand(this.NewBank));
        }

        public ActionCommand NewBankCommand { get; set; }


        #region Publics


        public void InitializeForAddEdit(bool addMode)
        {
            base.ValidationHelper.Enabled = !delayValidation;
            base.ValidationHelper.Reset();

            LoadBanksList();
            LoadExistingBankAccounts();

            if (base.ValidationHelper.Enabled)
                base.Validate();

            if (addMode)
            {
                this.BankAccountId = 0;
                this.AccountName = "";
                this.Bank = null;
                this.AccountNumber = "";
                this.SortCode = "";
                this.AccountOwner = "";
                this.OpenedDate = DateTime.Now.Date;
                this.ClosedDate = null;
                this.InitialRate = 0;
                this.LoginId = "";
                this.LoginUrl = "";
                this.MilestoneDate = null;
                this.MilestoneNotes = "";
                this.Notes = "";
                this.PasswordHint = "";
                this.PaysTaxableInterest = false;

            }

        }

        public string DialogTitle
        {
            get
            {
                return this.BankAccountId == 0 ? String.Format("Add new Bank Account") : String.Format("Edit Bank Account {0} ({1})", this.AccountName, this.Bank.Name);
            }
        }


        public ObservableCollection<BankItemViewModel> BankList
        {
            get
            {
                if (this.bankList == null)
                    this.bankList = new ObservableCollection<BankItemViewModel>();
                return this.bankList;
            }
        }


        int bankAccountId;
        public int BankAccountId 
        { 
            get { return this.bankAccountId; }
            set
            {
                if (this.bankAccountId != value)
                {
                    this.bankAccountId = value;
                    NotifyPropertyChangedAndValidate();
                }
            }
        }

        string accountName;
        [Required(ErrorMessage = "Account Name is mandatory")]
        public string AccountName
        {
            get { return accountName; }
            set
            {
                if (accountName != value)
                {
                    accountName = value;
                    NotifyPropertyChangedAndValidate();
                }
            }
        }

        public int BankId
        {
            get 
            { 
                return this.Bank==null?0:this.Bank.BankId; 
            }
            set 
            {   


//                this.Bank = this.BankList.First(b => b.BankId == value);
                NotifyPropertyChangedAndValidate();
            }
        }

        BankItemViewModel bank;
        public BankItemViewModel Bank
        {
            get 
            {
                if (bank == null)
                    bank = new BankItemViewModel();


                if (this.BankList!=null && !this.BankList.Contains(bank))
                {
                    BankItemViewModel find = this.BankList.FirstOrDefault(b => b.BankId == bank.BankId);
                    if (find != null)
                        bank = find;
                }
                
                return bank; 
            }
            set 
            {
                if (value != null)
                {
                    bank = value;

                    //bank = this.BankList.First(b => b.BankId == value.BankId);

                    //if (this.BankList.Contains(value))
                    //    bank = value;
                    //else
                    //    this.Bank = this.BankList.First(b => b.BankId == value.BankId);
                }
                else
                {
                    bank = value;
                }
                Debug.WriteLine("Bank - set({0})",value);
                NotifyPropertyChangedAndValidate();
            }
        }


        string sortCode;
        [MaxLength(6, ErrorMessage="Sort Code max length is 6")]
        public string SortCode
        {
            get { return sortCode; }
            set
            {
                sortCode = value; 
                NotifyPropertyChangedAndValidate();
            }
        }

        string accountNumber;
        [MaxLength(8, ErrorMessage = "Account Number max length is 8")]
        public string AccountNumber
        {
            get { return accountNumber; }
            set { accountNumber = value; NotifyPropertyChangedAndValidate(); }
        }



        string accountOwner;
        public string AccountOwner
        {
            get { return accountOwner; }
            set { accountOwner = value; NotifyPropertyChangedAndValidate(); }
        }

        bool paysTaxableInterest;
        public bool PaysTaxableInterest
        {
            get { return paysTaxableInterest; }
            set { paysTaxableInterest = value; NotifyPropertyChangedAndValidate(); }
        }


        string loginUrl;
        public string LoginUrl
        {
            get { return loginUrl; }
            set { loginUrl = value; NotifyPropertyChangedAndValidate(); }
        }

        string loginId;
        public string LoginId
        {
            get { return loginId; }
            set { loginId = value; NotifyPropertyChangedAndValidate(); }
        }

        string passwordHint;
        public string PasswordHint
        {
            get { return passwordHint; }
            set { passwordHint = value; NotifyPropertyChangedAndValidate(); }
        }


        DateTime? openedDate;
        [Required(ErrorMessage = "Date Opened is mandatory")]
        public DateTime? OpenedDate
        {
            get { return openedDate.HasValue ? openedDate.Value.Date : openedDate; }
            set { openedDate = value.HasValue ? value.Value.Date : value; NotifyPropertyChangedAndValidate(); }
        }

        DateTime? closedDate;
        public DateTime? ClosedDate
        {
            get { return closedDate.HasValue ? closedDate.Value.Date : closedDate; }
            set { closedDate = value.HasValue ? value.Value.Date : value; NotifyPropertyChangedAndValidate(); }
        }

        decimal? initialRate;
        public decimal? InitialRate
        {
            get { return initialRate; }
            set { initialRate = value; NotifyPropertyChangedAndValidate(); }
        }


        DateTime? milestoneDate;
        public DateTime? MilestoneDate
        {
            get { return milestoneDate; }
            set { milestoneDate = value; NotifyPropertyChanged(); }
        }

        string milestoneNotes;
        public string MilestoneNotes
        {
            get { return milestoneNotes; }
            set { milestoneNotes = value; NotifyPropertyChanged(); }
        }

        string notes;
        public string Notes
        {
            get { return notes; }
            set { notes = value; NotifyPropertyChanged(); }
        }
            
        #endregion


        
        #region Privates

        private void LoadBanksList()
        {
            Debug.WriteLine("LoadBanksList - start");
            BankList.Clear();
            bankRepository.ReadList().ForEach(b => BankList.Add(mapper.Map<BankItemViewModel>(b)));
            Debug.WriteLine("LoadBanksList - end");
        }

        private void LoadExistingBankAccounts()
        {
            existingBankAccounts = bankAccountRepository.ReadListDataIdName();
        }

        private void NewBank()
        {
            var editor = this.bankEditorViewModelFactory.Create();

            // prepare Editor
            editor.InitializeForAddEdit(true);

            // open Editor for adding
            while (this.dialogService.ShowDialogView(editor))
            {
                // map the Editor into an entity
                var newbank = mapper.Map<Bank>(editor);

                // save the entity and get result
                bool result = this.bankRepository.Add(newbank) > 0;

                if (result)
                {
                    LoadBanksList();

                    this.Bank = this.BankList.FirstOrDefault(b => b.BankId == newbank.BankId);

                    base.Validate();

                    break;
                }
            }

            this.bankEditorViewModelFactory.Release(editor);

        }


        #endregion

        #region Validation

        // Use annotations and/or ValidateDate() method
        // Add errors to ValidationHelper:
        // base.ValidationHelper.AddValidationMessage("Bank Name already exists", "Name");

        protected override void ValidateData()
        {
            Debug.WriteLine("ValidateData - start");

            // Bank Account exists
            if (existingBankAccounts != null)
            {
                string uniqueName = String.Format("{0} ({1})",this.AccountName,this.Bank==null?"":this.Bank.Name);
                if (existingBankAccounts.Exists(n => n.Name.Equals(uniqueName, StringComparison.CurrentCultureIgnoreCase) && n.Id != this.BankAccountId))
                {
                    base.ValidationHelper.AddValidationMessage("Bank + Account Name already exists", "AccountName");
                }
            }


            // Bank
            if (this.bankList != null)
            {
                if (!this.BankList.Contains(this.bank))
                {
                    base.ValidationHelper.AddValidationMessage("Invalid Bank", "Bank");
                    Debug.WriteLine("InvalidBank - (BankId={0}, qty={1})",this.bank.BankId,this.BankList.Count);
                
                }
            }

            Debug.WriteLine("ValidateData - end");
        }

        #endregion


    }
}
