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
using AutoMapper;
using Finances.WinClient.Factories;


namespace Finances.WinClient.ViewModels
{

    internal enum BankAccountFromTo
    {
        From,
        To
    }

    public class TransferEditorViewModel : EditorViewModelBase
    {
        bool delayValidation = false; // must be false until change logic around IsValid
        ObservableCollection<BankAccountItemViewModel> bankAccounts;
        List<DataIdName> existingTransfers;
        ObservableCollection<TransferCategory> categories;
        Transfer entity;

        readonly ITransferRepository transferRepository;
        readonly IBankAccountRepository bankAccountRepository;
        readonly IBankAccountAgent bankAccountAgent;

        public TransferEditorViewModel(
                ITransferRepository transferRepository,
                IBankAccountRepository bankAccountRepository,
                IBankAccountAgent bankAccountAgent,
                Transfer entity
            )
        {
            this.transferRepository = transferRepository;
            this.bankAccountRepository = bankAccountRepository;
            this.bankAccountAgent = bankAccountAgent;
            this.entity = entity;

            this.Amount.PropertyChanged += (s,e) =>
                {
                    base.Validate();
                };
            base.ValidationHelper.AddInstance(this.Amount);

            NewFromBankAccountCommand = base.AddNewCommand(new ActionCommand(this.NewFromBankAccount));
            NewToBankAccountCommand = base.AddNewCommand(new ActionCommand(this.NewToBankAccount));
        }




        #region Publics

        public ActionCommand NewFromBankAccountCommand { get; set; }
        public ActionCommand NewToBankAccountCommand { get; set; }

        public void InitializeForAddEdit(bool addMode)
        {
            base.ValidationHelper.Enabled = !delayValidation;
            base.ValidationHelper.Reset();

            LoadBankAccountList();
            LoadExistingTransfers();
            LoadTransferCategories();

            if (addMode)
            {
                this.Category = Categories.FirstOrDefault(c => c.Code == "NONE");
                this.StartDate = DateTime.Now.Date;
                this.IsEnabled = true;
            }

            this.Amount.Value = entity.Amount;
            this.Amount.ValueChangedAction = v => this.entity.Amount = v;

        }

        public string DialogTitle
        {
            get
            {
                return this.TransferId == 0 ? String.Format("Add new Transfer") : String.Format("Edit Transfer {0}", this.Name);
            }
        }


        public ObservableCollection<BankAccountItemViewModel> BankAccounts
        {
            get
            {
                if(this.bankAccounts==null)
                    this.bankAccounts = new ObservableCollection<BankAccountItemViewModel>();
                return this.bankAccounts;
            }
        }

        public ObservableCollection<TransferCategory> Categories
        {
            get
            {
                if (this.categories == null)
                    this.categories = new ObservableCollection<TransferCategory>();
                return this.categories;
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


        public int TransferId
        {
            get
            {
                return entity.TransferId;
            }
            set
            {
                entity.TransferId = value;
                NotifyPropertyChangedAndValidate();
            }
        }

        [Required(ErrorMessage = "Transfer Name is mandatory")]
        public string Name
        {
            get
            {
                return entity.Name;
            }
            set
            {
                entity.Name = value;
                NotifyPropertyChangedAndValidate();
            }
        }


        BankAccountItemViewModel fromBankAccount;
        public BankAccountItemViewModel FromBankAccount
        {
            get
            {
                if (fromBankAccount == null)
                {
                    if (entity.FromBankAccount == null || entity.FromBankAccount.BankAccountId==0)
                    {
                        fromBankAccount = BankAccountItemViewModel.Elsewhere;
                    }
                    else
                    {
                        fromBankAccount = new BankAccountItemViewModel(entity.FromBankAccount);
                    }
                }

                if (this.BankAccounts != null && !this.BankAccounts.Contains(fromBankAccount))
                {
                    BankAccountItemViewModel find = this.BankAccounts.FirstOrDefault(a => a != null && a.BankAccountId == fromBankAccount.BankAccountId);
                    if (find != null)
                    {
                        fromBankAccount = find;
                        entity.FromBankAccount = fromBankAccount.Entity;
                    }
                }

                return fromBankAccount;
            }
            set
            {
                if (fromBankAccount != value)
                {
                    fromBankAccount = value;
                    entity.FromBankAccount = fromBankAccount==null || fromBankAccount==BankAccountItemViewModel.Elsewhere ? null : fromBankAccount.Entity;

                    NotifyPropertyChangedAndValidate();
                }
            }
        }

        BankAccountItemViewModel toBankAccount;
        public BankAccountItemViewModel ToBankAccount
        {
            get
            {
                if (toBankAccount == null)
                {
                    if (entity.ToBankAccount == null || entity.ToBankAccount.BankAccountId==0)
                    {
                        toBankAccount = BankAccountItemViewModel.Elsewhere;
                    }
                    else
                    {
                        toBankAccount = new BankAccountItemViewModel(entity.ToBankAccount);
                    }
                }

                if (this.BankAccounts != null && !this.BankAccounts.Contains(toBankAccount))
                {
                    BankAccountItemViewModel find = this.BankAccounts.FirstOrDefault(a => a != null && a.BankAccountId == toBankAccount.BankAccountId);
                    if (find != null)
                    {
                        toBankAccount = find;
                        entity.ToBankAccount = toBankAccount.Entity;
                    }
                }


                return toBankAccount;
            }
            set
            {
                if (toBankAccount != value)
                {
                    toBankAccount = value;
                    entity.ToBankAccount = toBankAccount == null || toBankAccount == BankAccountItemViewModel.Elsewhere ? null : toBankAccount.Entity;

                    NotifyPropertyChangedAndValidate();
                }
            }
        }


        public TransferCategory Category 
        {
            get
            {
                if (this.Categories != null && !this.Categories.Contains(entity.Category))
                {
                    TransferCategory find = this.Categories.FirstOrDefault(c => c.TransferCategoryId == entity.Category.TransferCategoryId);
                    if (find != null)
                        entity.Category = find;
                }

                return entity.Category;
            }
            set
            {
                entity.Category = value;
                NotifyPropertyChangedAndValidate();
            }
        }


        InputDecimal amount;
        public InputDecimal Amount
        {
            get
            {
                if (amount == null)
                {
                    amount = new InputDecimal()
                    {
                        FormatString = "c2",
                        Mandatory = true//,
                    };
                }

                return amount;
            }
        }


        public decimal AmountTolerence
        {
            get
            {
                return entity.AmountTolerence;
            }
            set
            {
                entity.AmountTolerence = value;
                NotifyPropertyChangedAndValidate();
            }
        }


        [Required(ErrorMessage = "Start Date is mandatory")]
        public DateTime? StartDate
        {
            get 
            {
                return entity.StartDate == DateTime.MinValue ? (DateTime?)null : entity.StartDate;
            }
            set 
            {
                entity.StartDate = value.HasValue ? value.Value : DateTime.MinValue;
                NotifyPropertyChangedAndValidate();
            }
        }

        public DateTime? EndDate
        {
            get 
            {
                return entity.EndDate;
            }
            set 
            {
                entity.EndDate = value;
                NotifyPropertyChangedAndValidate(); 
            }
        }

        public string Frequency
        {
            get
            {
                if (entity.Frequency == null && this.Frequencies != null && this.Frequencies.Count() > 0)
                    entity.Frequency = this.Frequencies[0];
                return entity.Frequency;
            }
            set
            {
                entity.Frequency = value;
                NotifyPropertyChangedAndValidate();
            }
        }

        public bool IsEnabled
        {
            get
            {
                return entity.IsEnabled;
            }
            set
            {
                entity.IsEnabled = value;
                NotifyPropertyChangedAndValidate();
            }
        }

        #endregion

        #region Privates

        private void LoadBankAccountList()
        {
            BankAccounts.Clear();
            BankAccounts.Add(BankAccountItemViewModel.Elsewhere);
            bankAccountRepository.ReadList().ForEach(a => BankAccounts.Add(new BankAccountItemViewModel(a)));
        }

        private void LoadExistingTransfers()
        {
            Task.Factory.StartNew(() =>
                {
                    existingTransfers = transferRepository.ReadListDataIdName();
                });

        }

        private void LoadTransferCategories()
        {
            Categories.Clear();
            transferRepository.ReadListTransferCategories().ForEach(tc => Categories.Add(tc));
        }

        private void NewFromBankAccount()
        {
            NewBankAccount(BankAccountFromTo.From);
        }

        private void NewToBankAccount()
        {
            NewBankAccount(BankAccountFromTo.To);
        }


        private void NewBankAccount(BankAccountFromTo fromto)
        {
            int id = this.bankAccountAgent.Add();
            if (id > 0)
            {
                var fromBankAccountId = this.FromBankAccount.BankAccountId;
                var toBankAccountId = this.ToBankAccount.BankAccountId;

                LoadBankAccountList();

                if (fromto == BankAccountFromTo.From)
                {
                    this.FromBankAccount = this.BankAccounts.FirstOrDefault(a => a.BankAccountId == id);
                    this.ToBankAccount = this.BankAccounts.FirstOrDefault(a => a.BankAccountId == toBankAccountId);
                }
                else
                {
                    this.FromBankAccount = this.BankAccounts.FirstOrDefault(a => a.BankAccountId == fromBankAccountId);
                    this.ToBankAccount = this.BankAccounts.FirstOrDefault(a => a.BankAccountId == id);
                }

                //base.Validate();

            }
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
                base.ValidationHelper.AddValidationMessage("Amount is invalid", this.Amount);
            }
            else if (!amount.HasValue)
            {
                base.ValidationHelper.AddValidationMessage("Amount is mandatory", this.Amount);
            }
            else if (amount.Value<=0)
            {
                base.ValidationHelper.AddValidationMessage("Amount must be > 0", this.Amount);
            }

        }

        #endregion


    }
}
