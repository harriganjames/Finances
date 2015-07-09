using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Core.Wpf;
using Finances.WinClient.DomainServices;
using Finances.WinClient.Factories;

namespace Finances.WinClient.ViewModels
{
    public interface IBankAccountListViewModel 
    {
        void Open();
        void Close();
    }

    public class BankAccountListViewModel : ListViewModelBase<BankAccountItemViewModel>, IBankAccountListViewModel
    {
        readonly IBankAccountRepository bankAccountRepository;
        readonly IBankAccountAgent bankAccountAgent;

        Dictionary<int, BankAccount> bankAccounts = new Dictionary<int, BankAccount>();

        public BankAccountListViewModel(
                        IBankAccountRepository bankAccountRepository,
                        IBankAccountAgent bankAccountAgent
                        )
        {
            this.bankAccountRepository = bankAccountRepository;
            this.bankAccountAgent = bankAccountAgent;

            ReloadCommand = base.AddNewCommand(new ActionCommand(Reload));
            AddCommand = base.AddNewCommand(new ActionCommand(Add));
            EditCommand = base.AddNewCommand(new ActionCommand(Edit, CanEdit));
            DeleteCommand = base.AddNewCommand(new ActionCommand(Delete, CanDelete));
        }


        #region Commmands

        public ActionCommand ReloadCommand { get; set; }
        public ActionCommand AddCommand { get; set; }
        public ActionCommand EditCommand { get; set; }
        public ActionCommand DeleteCommand { get; set; }

        #endregion

        #region PublicProperties

        public ObservableCollection<BankAccountItemViewModel> BankAccounts
        {
            get
            {
                return base.DataList;
            }
        }

        #endregion


        public override void WorkspaceOpened()
        {
            Open();
        }

        public override void WorkspaceClosed()
        {
            Close();
        }


        public void Open()
        {
            LoadBankAccounts();
        }

        public void Close()
        {
            base.DataList.Clear();
        }

        private void Reload()
        {
            LoadBankAccounts();
        }

        private void LoadBankAccounts()
        {
            base.IsBusy = true;

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) =>
            {
                bankAccounts.Clear();

                var entities = this.bankAccountRepository.ReadList();

                if (entities != null)
                    entities.ForEach(a => bankAccounts.Add(a.BankAccountId, a));

            };
            bw.RunWorkerCompleted += (s, e) =>
            {
                base.DataList.Clear();
                if (bankAccounts != null)
                {
                    bankAccounts.Values.ToList().ForEach(a =>
                    {
                        base.DataList.Add(new BankAccountItemViewModel(a));
                    });
                }

                base.IsBusy = false;
            };

            bw.RunWorkerAsync();

        }

        private void Add()
        {
            int id = this.bankAccountAgent.Add();
            if (id > 0)
            {
                var entity = this.bankAccountRepository.Read(id);

                this.bankAccounts.Add(entity.BankAccountId, entity);

                var vm = new BankAccountItemViewModel(entity);

                base.DataList.Add(vm);
                base.DataList.ToList().ForEach(i => i.IsSelected = false);
                vm.IsSelected = true;
            }

        }


        private bool CanEdit()
        {
            return base.GetQtySelected() == 1;
        }
        private void Edit()
        {
            var vm = base.GetSelectedItems().First();

            int id = vm.BankAccountId;

            bool result = this.bankAccountAgent.Edit(id);

            if (result)
            {
                this.bankAccounts[id] = this.bankAccountRepository.Read(id);

                vm.Entity = this.bankAccounts[id];
            }

        }


        private bool CanDelete()
        {
            return base.GetQtySelected() > 0;
        }
        private void Delete()
        {
            var vms = base.GetSelectedItems().ToList();
            List<int> ids = vms.Select(vm => vm.BankAccountId).ToList();

            if (this.bankAccountAgent.Delete(ids))
            {
                base.IsBusy = true;

                foreach (var vm in vms)
                {
                    base.DataList.Remove(vm);
                }

                base.IsBusy = false;
            }
        }


        public override string Caption
        {
            get
            {
                return "Bank Accounts";
            }
        }

    }

}
