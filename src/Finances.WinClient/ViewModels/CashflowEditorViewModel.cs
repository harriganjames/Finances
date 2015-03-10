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

    public class CashflowEditorViewModel : EditorViewModelBase
    {
        bool delayValidation = false; // must be false until change logic around IsValid
        ObservableCollection<BankAccountItemViewModel> bankAccounts;
        ObservableCollection<CashflowBankAccountItemViewModel> cashflowBankAccounts;
        List<DataIdName> existingCashflows;
        Dictionary<int, BankAccount> allBankAccounts = new Dictionary<int,BankAccount>();
        Cashflow entity;

        readonly ICashflowRepository cashflowRepository;
        readonly IMappingEngine mapper;
        readonly IBankAccountRepository bankAccountRepository;
        readonly IDialogService dialogService;

        public CashflowEditorViewModel(
                ICashflowRepository cashflowRepository,
                IMappingEngine mapper,
                IBankAccountRepository bankAccountRepository,
                IDialogService dialogService
            )
        {
            this.cashflowRepository = cashflowRepository;
            this.mapper = mapper;
            this.bankAccountRepository = bankAccountRepository;
            this.dialogService = dialogService;

            this.OpeningBalance.PropertyChanged += (s,e) =>
                {
                    base.Validate();
                };
            base.ValidationHelper.AddInstance(this.OpeningBalance);

        }




        #region Publics


        public void InitializeForAddEdit(bool addMode, Cashflow entity)
        {
            base.ValidationHelper.Enabled = !delayValidation;
            base.ValidationHelper.Reset();

            this.entity = entity;

            LoadBankAccountList();
            LoadExistingCashflows();

            if (addMode)
            {
                this.StartDate = DateTime.Now.Date;
            }

            this.OpeningBalance.Value = entity.OpeningBalance;
            this.OpeningBalance.ValueChangedAction = v => this.entity.OpeningBalance = v;

        }

        public string DialogTitle
        {
            get
            {
                return this.CashflowId == 0 ? String.Format("Add new Cashflow") : String.Format("Edit Cashflow {0}", this.Name);
            }
        }


        // used by View to capture accounts
        public ObservableCollection<BankAccountItemViewModel> BankAccounts
        {
            get
            {
                if(this.bankAccounts==null)
                    this.bankAccounts = new ObservableCollection<BankAccountItemViewModel>();
                return this.bankAccounts;
            }
        }

        // use by mapper to get accounts in/out
        //public ObservableCollection<CashflowBankAccountItemViewModel> CashflowBankAccounts
        //{
        //    get
        //    {
        //        if (this.cashflowBankAccounts == null)
        //            this.cashflowBankAccounts = new ObservableCollection<CashflowBankAccountItemViewModel>();
        //        return this.cashflowBankAccounts;
        //    }
        //}


        //int cashflowId;
        public int CashflowId
        {
            get { return entity.CashflowId; }
            set { entity.CashflowId = value; NotifyPropertyChangedAndValidate(); }
        }


        string name;
        [Required(ErrorMessage = "Cashflow Name is mandatory")]
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



        InputDecimal openingBalance;
        // = new InputDecimal() 
            //{ 
            //    FormatString = "c2", 
            //    Mandatory = true
            //};
        public InputDecimal OpeningBalance
        {
            get
            {
                if (openingBalance == null)
                {
                    openingBalance = new InputDecimal()
                    {
                        FormatString = "c2",
                        Mandatory = true//,
                        //ValueChangedAction = v => this.entity.OpeningBalance = v,
                    };
                }
                return openingBalance;
            }
            //set
            //{
            //    openingBalance = value;
            //    openingBalance.FormatString = "n2";
            //    openingBalance.Mandatory = true;
            //    NotifyPropertyChangedAndValidate();
            //}
        }



        //DateTime? startDate;
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


        #endregion

        #region Privates

        private void LoadBankAccountList()
        {
            // load dictionary of BankAccount entities
            allBankAccounts.Clear();
            bankAccountRepository.ReadList().ToList().ForEach(a => allBankAccounts.Add(a.BankAccountId,a));

            // populate collection for View
            BankAccounts.Clear();
            allBankAccounts.Values.ToList().ForEach(a => BankAccounts.Add(mapper.Map<BankAccountItemViewModel>(a)));

            // Flag selected items
            foreach (var cfa in this.entity.CashflowBankAccounts)
            {
                var a = this.BankAccounts.FirstOrDefault(ba => ba.BankAccountId == cfa.BankAccount.BankAccountId);
                if (a != null)
                    a.IsSelected = true;
            }
       
        }

        private void LoadExistingCashflows()
        {
            Task.Factory.StartNew(() =>
                {
                    existingCashflows = cashflowRepository.ReadListDataIdName();
                });

        }


        public override void DialogOkClicked()
        {
            // copy selected accounts to entity.CashflowBankAccounts

            // need to copy incrementally
            // then check this in repo


            // add new accounts
            this.BankAccounts.Where(a => a.IsSelected).ToList().ForEach(s =>
            {
                if (entity.CashflowBankAccounts.Count(e => e.BankAccount.BankAccountId == s.BankAccountId) == 0)
                {
                    entity.CashflowBankAccounts.Add(new CashflowBankAccount()
                    {
                        BankAccount = allBankAccounts[s.BankAccountId]
                    });
                }
            }
            );

            // remove deleted accounts
            entity.CashflowBankAccounts.ToList().ForEach(e =>
            {
                if (BankAccounts.Count(a => a.BankAccountId == e.BankAccount.BankAccountId && a.IsSelected)==0)
                {
                    entity.CashflowBankAccounts.Remove(e);
                }
            }
            );



        }


        #endregion

        #region Validation

        // Use annotations and/or ValidateDate() method
        // Add errors to ValidationHelper:
        // base.ValidationHelper.AddValidationMessage("Bank Name already exists", "Name");

        protected override void ValidateData()
        {
            //Cashflow exists
            if (existingCashflows != null)
            {
                string uniqueName = String.Format("{0}", this.Name);
                if (existingCashflows.Exists(n => n.Name.Equals(uniqueName, StringComparison.CurrentCultureIgnoreCase) && n.Id != this.CashflowId))
                {
                    base.ValidationHelper.AddValidationMessage("Cashflow Name already exists", "Name");
                }
            }


            if (!openingBalance.IsNumeric)
            {
                base.ValidationHelper.AddValidationMessage("OpeningBalance is invalid", this.OpeningBalance);
            }
            else if (!openingBalance.HasValue)
            {
                base.ValidationHelper.AddValidationMessage("OpeningBalance is mandatory", this.OpeningBalance);
            }

        }

        #endregion


    }
}
