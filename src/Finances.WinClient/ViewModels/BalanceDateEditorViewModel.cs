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
using System.ComponentModel;

namespace Finances.WinClient.ViewModels
{
    public class BalanceDateEditorViewModel : ValidationViewModelBase
    {
        bool delayValidation = false; // must be false until change logic around IsValid
        List<DataIdName> existingEntities;
        BalanceDate entity;

        readonly IBankAccountRepository bankAccountRepository;
        readonly IBalanceDateRepository balanceDateRepository;


        public BalanceDateEditorViewModel(
                        IBankAccountRepository bankAccountRepository,
                        IBalanceDateRepository balanceDateRepository,
                        BalanceDate entity
                        )
        {
            this.bankAccountRepository = bankAccountRepository;
            this.balanceDateRepository = balanceDateRepository;
            this.entity = entity;
        }


        #region Publics


        public void InitializeForAddEdit(bool addMode)
        {
            base.ValidationHelper.Enabled = true; 
            base.ValidationHelper.Reset();

            LoadExistingEntities();
            LoadBankAccounts();

            foreach (var item in BalanceDateBankAccounts)
            {
                base.AddValidationInstance(item);
            }

            if (base.ValidationHelper.Enabled)
                base.Validate();

            if (addMode)
            {
                this.DateOfBalance = DateTime.Now.Date;
            }

        }

        public string DialogTitle
        {
            get
            {
                return this.BalanceDateId == 0 ? String.Format("Add new Balance Date") : String.Format("Edit Balance Date {0:yyyy-MM-dd}", this.DateOfBalance);
            }
        }



        public int BalanceDateId
        { 
            get { return entity.BalanceDateId; }
            set
            {
                if (entity.BalanceDateId != value)
                {
                    entity.BalanceDateId = value;
                    NotifyPropertyChangedAndValidate();
                }
            }
        }

        [Required(ErrorMessage = "Date is mandatory")]
        public DateTime DateOfBalance
        {
            get { return entity.DateOfBalance; }
            set
            {
                if (entity.DateOfBalance != value)
                {
                    entity.DateOfBalance = value;
                    NotifyPropertyChangedAndValidate();
                }
            }
        }


        ObservableCollectionSafe<BalanceDateBankAccountItemViewModel> balanceDateBankAccounts;
        public ObservableCollectionSafe<BalanceDateBankAccountItemViewModel> BalanceDateBankAccounts
        {
            get
            {
                if (balanceDateBankAccounts == null)
                    balanceDateBankAccounts = new ObservableCollectionSafe<BalanceDateBankAccountItemViewModel>();
                return balanceDateBankAccounts;
            }
        }

        public override void DialogOkClicked()
        {
            foreach (var ba in BalanceDateBankAccounts)
            {
                // add new entities
                if(ba.BalanceDateBankAccountId==0 && ba.BalanceAmount.HasValue)
                {
                    entity.BalanceDateBankAccounts.Add(ba.Entity);
                }

                //remove unwanted entities
                if(ba.BalanceDateBankAccountId!=0 && !ba.BalanceAmount.HasValue)
                {
                    entity.BalanceDateBankAccounts.Remove(ba.Entity);
                }
            }
        }

        #endregion



        #region Privates


        private void LoadExistingEntities()
        {
            existingEntities = balanceDateRepository.ReadListDataIdName();
        }

        Dictionary<int, BankAccount> allBankAccounts = new Dictionary<int, BankAccount>();

        void LoadBankAccounts()
        {
            // load dictionary of BankAccount entities
            allBankAccounts.Clear();
            bankAccountRepository.ReadList().ToList().ForEach(a => allBankAccounts.Add(a.BankAccountId, a));

            // load from entity
            foreach (var ba in entity.BalanceDateBankAccounts)
            {
                BalanceDateBankAccounts.Add(new BalanceDateBankAccountItemViewModel(ba));
            }

            // load remaining bank accounts

            //var sd = allBankAccounts.Values.Where(x => x.BankAccountId==1);
            var bankAccountsInEntity = allBankAccounts.Values.Where(x => entity.BalanceDateBankAccounts.Select(b => b.BankAccount.BankAccountId).Contains(x.BankAccountId));

            var remaining = allBankAccounts.Values.Except(bankAccountsInEntity);

            //var remaining = allBankAccounts.Values.Except(entity.BalanceDateBankAccounts.Select(e => e.BankAccount),new BankAccount());


            foreach (var ba in remaining)
            {
                BalanceDateBankAccounts.Add(new BalanceDateBankAccountItemViewModel(new BalanceDateBankAccount() { BankAccount = ba }));
            }



        }


        #endregion

        #region Validation

            // Use annotations and/or ValidateData() method
            // Add errors to ValidationHelper:
            // base.ValidationHelper.AddValidationMessage("Bank Name already exists", "Name");

        protected override void ValidateData()
        {
            Debug.WriteLine("ValidateData - start");

            // Date exists
            if (existingEntities != null)
            {
                string uniqueName = String.Format("{0}",DateOfBalance.ToString());
                if (existingEntities.Exists(n => n.Name.Equals(uniqueName, StringComparison.CurrentCultureIgnoreCase) && n.Id != this.BalanceDateId))
                {
                    base.ValidationHelper.AddValidationMessage("Date already exists", "DateOfBalance");
                }
            }

            if(!this.BalanceDateBankAccounts.Any(bda=>bda.BalanceAmount.HasValue))
            {
                base.ValidationHelper.AddValidationMessage("At least one amount must be entered", "");
            }


            Debug.WriteLine("ValidateData - end");
        }

        #endregion


    }
}
