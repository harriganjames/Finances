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

namespace Finances.WinClient.ViewModels
{
    public interface IBankAccountsViewModelNoService : IListViewModelBase<IBankAccountItemViewModel>
    {
        ActionCommand AddCommand { get; set; }
        ActionCommand EditCommand { get; set; }
        ActionCommand DeleteCommand { get; set; }
        ActionCommand ReloadCommand { get; set; }
        ObservableCollection<IBankAccountItemViewModel> BankAccounts { get; }
    }

    public class BankAccountsViewModelNoService : ListViewModelBase<IBankAccountItemViewModel>, IBankAccountsViewModelNoService
    {
        readonly IBankAccountRepository bankAccountRepository;
        readonly IMappingEngine mapper;
        readonly IDialogService dialogService;
        readonly IBankAccountEditorViewModel bankAccountEditorViewModel;

        Dictionary<int, BankAccount> bankAccounts = new Dictionary<int, BankAccount>();

        public BankAccountsViewModelNoService(
                        IBankAccountRepository bankAccountRepository,
                        IMappingEngine mapper,
                        IDialogService dialogService,
                        IBankAccountEditorViewModel bankAccountEditorViewModel)
        {
            this.bankAccountRepository = bankAccountRepository;
            this.mapper = mapper;
            this.dialogService = dialogService;
            this.bankAccountEditorViewModel = bankAccountEditorViewModel;

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

        public ObservableCollection<IBankAccountItemViewModel> BankAccounts
        {
            get
            {
                return base.DataList;
            }
        }

        #endregion


        public override void WorkspaceOpened()
        {
            LoadBankAccounts();
        }

        public override void WorkspaceClosed()
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
                        base.DataList.Add(mapper.Map<BankAccountItemViewModel>(a));
                    });
                }

                base.IsBusy = false;
            };

            bw.RunWorkerAsync();

        }



        private void Add()
        {
            this.bankAccountEditorViewModel.InitializeForAddEdit(true);

            while (this.dialogService.ShowDialogView(this.bankAccountEditorViewModel))
            {
                var entity = mapper.Map<BankAccount>(this.bankAccountEditorViewModel);

                bool result = this.bankAccountRepository.Add(entity) > 0;

                if (result)
                {
                    this.bankAccounts.Add(entity.BankAccountId, entity);

                    var vm = mapper.Map<BankAccountItemViewModel>(entity);

                    base.DataList.Add(vm);
                    base.DataList.ToList().ForEach(i => i.IsSelected = false);
                    vm.IsSelected = true;
                    break;
                }
            }

        }


        private bool CanEdit()
        {
            return base.GetQtySelected() == 1;
        }
        private void Edit()
        {
            IBankAccountItemViewModel vm = base.GetSelectedItems().First();

            BankAccount bankAccount = this.bankAccounts[vm.BankAccountId];

            mapper.Map<BankAccount, BankAccountEditorViewModel>(bankAccount, this.bankAccountEditorViewModel as BankAccountEditorViewModel);

            this.bankAccountEditorViewModel.InitializeForAddEdit(false);

            while (this.dialogService.ShowDialogView(this.bankAccountEditorViewModel))
            {
                var upd = mapper.Map<BankAccount>(this.bankAccountEditorViewModel);

                bool result = this.bankAccountRepository.Update(upd);
                if (result)
                {
                    this.bankAccounts[vm.BankAccountId] = this.bankAccountRepository.Read(vm.BankAccountId);
                    mapper.Map<BankAccount, BankAccountItemViewModel>(this.bankAccounts[vm.BankAccountId], vm as BankAccountItemViewModel);
                    break;
                }

            }
        }


        private bool CanDelete()
        {
            return base.GetQtySelected() > 0;
        }
        private void Delete()
        {
            string title;
            string message;
            List<IBankAccountItemViewModel> sel = base.GetSelectedItems().ToList();

            if (sel.Count() == 0)
            {
                throw new Exception("Delete Bank Account: nothing selected");
            }

            if (sel.Count() == 1)
            {
                title = "Delete Bank Account";
                message = String.Format("Please confirm deletion of Bank Account: {0}", String.Format("{0} - {1}", sel.First().Bank.Name, sel.First().AccountName));
            }
            else
            {
                title = "Delete Bank Accounts";
                message = String.Format("Please confirm deletion of {0} Bank Accounts?", sel.Count());
            }


            if (this.dialogService.ShowMessageBox(title, message, MessageBoxButtonEnum.YesNo) == MessageBoxResultEnum.Yes)
            {
                base.IsBusy = true;

                foreach (var vm in sel)
                {
                    BankAccount entity = mapper.Map<BankAccount>(vm);
                    if (this.bankAccountRepository.Delete(entity))
                    {
                        base.DataList.Remove(vm);
                        this.bankAccounts.Remove(entity.BankAccountId);
                    }
                    else
                        break;
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
