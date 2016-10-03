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
    public class BankAccountEditorViewModel : ValidationViewModelBase
    {
        bool delayValidation = false; // must be false until change logic around IsValid
        ObservableCollection<BankItemViewModel> bankList;
        List<DataIdName> existingBankAccounts;
        BankAccount entity;

        readonly IBankAccountRepository bankAccountRepository;
        readonly IBankRepository bankRepository;
        readonly IBankAgent bankAgent;


        public BankAccountEditorViewModel(
                        IBankAccountRepository bankAccountRepository,
                        IBankRepository bankRepository,
                        IBankAgent bankAgent,
                        BankAccount entity
                        )
        {
            this.bankRepository = bankRepository;
            this.bankAccountRepository = bankAccountRepository;
            this.bankAgent = bankAgent;
            this.entity = entity;

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
                this.OpenedDate = DateTime.Now.Date;
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


        public int BankAccountId 
        { 
            get { return entity.BankAccountId; }
            set
            {
                if (entity.BankAccountId != value)
                {
                    entity.BankAccountId = value;
                    NotifyPropertyChangedAndValidate();
                }
            }
        }

        [Required(ErrorMessage = "Account Name is mandatory")]
        public string AccountName
        {
            get { return entity.Name; }
            set
            {
                if (entity.Name != value)
                {
                    entity.Name = value;
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
        }

        BankItemViewModel bank;
        public BankItemViewModel Bank
        {
            get 
            {
                if (bank == null)
                    bank = new BankItemViewModel(entity.Bank);


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

                    entity.Bank = bank.Entity;

                }
                else
                {
                    bank = value;
                }
                Debug.WriteLine("Bank - set({0})",value);
                NotifyPropertyChangedAndValidate();
            }
        }


        [MaxLength(6, ErrorMessage="Sort Code max length is 6")]
        public string SortCode
        {
            get { return entity.SortCode; }
            set
            {
                entity.SortCode = value; 
                NotifyPropertyChangedAndValidate();
            }
        }

        [MaxLength(8, ErrorMessage = "Account Number max length is 8")]
        public string AccountNumber
        {
            get { return entity.AccountNumber; }
            set { entity.AccountNumber = value; NotifyPropertyChangedAndValidate(); }
        }



        public string AccountOwner
        {
            get { return entity.AccountOwner; }
            set { entity.AccountOwner = value; NotifyPropertyChangedAndValidate(); }
        }

        public bool PaysTaxableInterest
        {
            get { return entity.PaysTaxableInterest; }
            set { entity.PaysTaxableInterest = value; NotifyPropertyChangedAndValidate(); }
        }


        public string LoginUrl
        {
            get { return entity.LoginURL; }
            set { entity.LoginURL = value; NotifyPropertyChangedAndValidate(); }
        }

        public string LoginId
        {
            get { return entity.LoginID; }
            set { entity.LoginID = value; NotifyPropertyChangedAndValidate(); }
        }

        public string PasswordHint
        {
            get { return entity.PasswordHint; }
            set { entity.PasswordHint = value; NotifyPropertyChangedAndValidate(); }
        }


        [Required(ErrorMessage = "Date Opened is mandatory")]
        public DateTime OpenedDate
        {
            get { return entity.OpenedDate; }
            set { entity.OpenedDate = value; NotifyPropertyChangedAndValidate(); }
        }

        public DateTime? ClosedDate
        {
            get { return entity.ClosedDate; }
            set { entity.ClosedDate = value; NotifyPropertyChangedAndValidate(); }
        }

        public decimal? InitialRate
        {
            get { return entity.InitialRate; }
            set { entity.InitialRate = value; NotifyPropertyChangedAndValidate(); }
        }

        public DateTime? MilestoneDate
        {
            get { return entity.MilestoneDate; }
            set { entity.MilestoneDate = value; NotifyPropertyChanged(); }
        }

        public string MilestoneNotes
        {
            get { return entity.MilestoneNotes; }
            set { entity.MilestoneNotes = value; NotifyPropertyChanged(); }
        }

        public string Notes
        {
            get { return entity.Notes; }
            set { entity.Notes = value; NotifyPropertyChanged(); }
        }
            
        #endregion


        
        #region Privates

        private void LoadBanksList()
        {
            Debug.WriteLine("LoadBanksList - start");
            BankList.Clear();
            bankRepository.ReadList().ForEach(b => BankList.Add(new BankItemViewModel(b)));
            Debug.WriteLine("LoadBanksList - end");
        }

        private void LoadExistingBankAccounts()
        {
            existingBankAccounts = bankAccountRepository.ReadListDataIdName();
        }



        private void NewBank()
        {
            int id = this.bankAgent.Add();
            if (id > 0)
            {
                LoadBanksList();

                this.Bank = this.BankList.FirstOrDefault(b => b.BankId == id);

                base.Validate();

            }
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
