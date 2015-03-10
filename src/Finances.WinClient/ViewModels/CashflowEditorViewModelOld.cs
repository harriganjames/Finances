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

    public class CashflowEditorViewModelOld : EditorViewModelBase
    {
        bool delayValidation = false; // must be false until change logic around IsValid
        ObservableCollection<BankAccountItemViewModel> bankAccounts;
        ObservableCollection<CashflowBankAccountItemViewModel> cashflowBankAccounts;
        List<DataIdName> existingCashflows;

        readonly ICashflowRepository transferRepository;
        readonly IMappingEngine mapper;
        readonly IBankAccountRepository bankAccountRepository;
        readonly IDialogService dialogService;

        public CashflowEditorViewModelOld(
                ICashflowRepository transferRepository,
                IMappingEngine mapper,
                IBankAccountRepository bankAccountRepository,
                IDialogService dialogService
            )
        {
            this.transferRepository = transferRepository;
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


        public void InitializeForAddEdit(bool addMode)
        {
            base.ValidationHelper.Enabled = !delayValidation;
            base.ValidationHelper.Reset();

            LoadBankAccountList();
            LoadExistingCashflows();

            if (addMode)
            {
                this.StartDate = DateTime.Now.Date;
            }

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
                //if(this.bankAccounts==null)
                //    this.bankAccounts = new ObservableCollection<BankAccountItemViewModel>();
                return this.bankAccounts;
            }
        }

        // use by mapper to get accounts in/out
        public ObservableCollection<CashflowBankAccountItemViewModel> CashflowBankAccounts
        {
            get
            {
                if (this.cashflowBankAccounts == null)
                    this.cashflowBankAccounts = new ObservableCollection<CashflowBankAccountItemViewModel>();
                return this.cashflowBankAccounts;
            }
        }


        int cashflowId;
        public int CashflowId
        {
            get { return cashflowId; }
            set { cashflowId = value; NotifyPropertyChangedAndValidate(); }
        }


        string name;
        [Required(ErrorMessage = "Cashflow Name is mandatory")]
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



        InputDecimal openingBalance = new InputDecimal() { FormatString = "c2", Mandatory = true };
        public InputDecimal OpeningBalance
        {
            get
            {
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





        DateTime? startDate;
        [Required(ErrorMessage = "Start Date is mandatory")]
        public DateTime? StartDate
        {
            get { return startDate.HasValue ? startDate.Value.Date : startDate; }
            set { startDate = value.HasValue ? value.Value.Date : value; NotifyPropertyChangedAndValidate(); }
        }


        #endregion

        #region Privates

        private void LoadBankAccountList()
        {
            BankAccounts.Clear();
            //BankAccounts.Add(BankAccountItemViewModel.Elsewhere);
            bankAccountRepository.ReadList().ForEach(a => BankAccounts.Add(mapper.Map<BankAccountItemViewModel>(a)));

            foreach (var cfa in this.CashflowBankAccounts)
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
                    existingCashflows = transferRepository.ReadListDataIdName();
                });

        }


        public override void DialogOkClicked()
        {
            base.DialogOkClicked();
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
