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
        List<DataIdName> existingCashflows;
        Dictionary<int, BankAccount> allBankAccounts = new Dictionary<int,BankAccount>();
        Cashflow entity;

        readonly ICashflowRepository cashflowRepository;
        //readonly IRepositoryWrite<Cashflow> cashflowRepositoryWrite;
        //readonly IRepositoryRead<Cashflow> cashflowRepositoryRead;
        readonly IBankAccountRepository bankAccountRepository;

        public CashflowEditorViewModel(
                ICashflowRepository cashflowRepository,
                IBankAccountRepository bankAccountRepository,
                Cashflow entity
            )
        {
            this.cashflowRepository = cashflowRepository;
            this.bankAccountRepository = bankAccountRepository;
            this.entity = entity;

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

            this.OpeningBalance.Value = entity.OpeningBalance;
            this.OpeningBalance.ValueChangedAction = v => this.entity.OpeningBalance = v;

            this.AllAccounts = entity.CashflowBankAccounts.Count == 0;

        }

        public string DialogTitle
        {
            get
            {
                return this.CashflowId == 0 ? String.Format("Add new Cashflow") : String.Format("Edit Cashflow {0}", this.Name);
            }
        }


        bool allAccounts;
        public bool AllAccounts
        {
            get
            {
                return allAccounts;
            }
            set
            {
                allAccounts = value;
                NotifyPropertyChangedAndValidate();
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


        public int CashflowId
        {
            get { return entity.CashflowId; }
            set { entity.CashflowId = value; NotifyPropertyChangedAndValidate(); }
        }


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
        public InputDecimal OpeningBalance
        {
            get
            {
                if (openingBalance == null)
                {
                    openingBalance = new InputDecimal()
                    {
                        FormatString = "c2",
                        Mandatory = true,
                    };
                }
                return openingBalance;
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


        #endregion


        #region Privates

        private void LoadBankAccountList()
        {
            // load dictionary of BankAccount entities
            allBankAccounts.Clear();
            bankAccountRepository.ReadList().ToList().ForEach(a => allBankAccounts.Add(a.BankAccountId,a));

            // populate collection for View
            BankAccounts.Clear();
            allBankAccounts.Values.ToList().ForEach(a => BankAccounts.Add(new BankAccountItemViewModel(a)));

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

            if (this.AllAccounts)
            {
                entity.CashflowBankAccounts.Clear();
            }
            else
            {

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
                    if (BankAccounts.Count(a => a.BankAccountId == e.BankAccount.BankAccountId && a.IsSelected) == 0)
                    {
                        entity.CashflowBankAccounts.Remove(e);
                    }
                }
                );
            }


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
