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
    public interface IBankAccountsViewModel //: IListViewModelBase<BankAccountItemViewModel>
    {
        //ActionCommand AddCommand { get; set; }
        //ActionCommand EditCommand { get; set; }
        //ActionCommand DeleteCommand { get; set; }
        //ActionCommand ReloadCommand { get; set; }
        //ObservableCollection<BankAccountItemViewModel> BankAccounts { get; }
        void Open();
        void Close();
    }

    public class BankAccountsViewModel : ListViewModelBase<BankAccountItemViewModel>, IBankAccountsViewModel
    {
        readonly IBankAccountRepository bankAccountRepository;
        readonly IMappingEngine mapper;
        readonly IDialogService dialogService;
        readonly IBankAccountEditorViewModelFactory bankAccountEditorViewModelFactory;

        Dictionary<int, BankAccount> bankAccounts = new Dictionary<int, BankAccount>();

        public BankAccountsViewModel(
                        IBankAccountRepository bankAccountRepository,
                        IMappingEngine mapper,
                        IDialogService dialogService,
                        IBankAccountEditorViewModelFactory bankAccountEditorViewModelFactory)
        {
            this.bankAccountRepository = bankAccountRepository;
            this.mapper = mapper;
            this.dialogService = dialogService;
            this.bankAccountEditorViewModelFactory = bankAccountEditorViewModelFactory;

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
                        base.DataList.Add(mapper.Map<BankAccountItemViewModel>(a));
                    });
                }

                base.IsBusy = false;
            };

            bw.RunWorkerAsync();

        }



        private void Add()
        {
            var editor = this.bankAccountEditorViewModelFactory.Create();

            editor.InitializeForAddEdit(true);

            while (this.dialogService.ShowDialogView(editor))
            {
                var entity = mapper.Map<BankAccount>(editor);

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

            this.bankAccountEditorViewModelFactory.Release(editor);

        }


        private bool CanEdit()
        {
            return base.GetQtySelected() == 1;
        }
        private void Edit()
        {
            BankAccountItemViewModel vm = base.GetSelectedItems().First();

            BankAccount bankAccount = this.bankAccounts[vm.BankAccountId];

            var editor = this.bankAccountEditorViewModelFactory.Create();

            mapper.Map<BankAccount, BankAccountEditorViewModel>(bankAccount, editor as BankAccountEditorViewModel);

            editor.InitializeForAddEdit(false);

            while (this.dialogService.ShowDialogView(editor))
            {
                var upd = mapper.Map<BankAccount>(editor);

                bool result = this.bankAccountRepository.Update(upd);
                if (result)
                {
                    this.bankAccounts[vm.BankAccountId] = this.bankAccountRepository.Read(vm.BankAccountId);
                    mapper.Map<BankAccount, BankAccountItemViewModel>(this.bankAccounts[vm.BankAccountId], vm as BankAccountItemViewModel);
                    break;
                }

            }

            this.bankAccountEditorViewModelFactory.Release(editor);
        }


        private bool CanDelete()
        {
            return base.GetQtySelected() > 0;
        }
        private void Delete()
        {
            string title;
            string message;
            List<BankAccountItemViewModel> sel = base.GetSelectedItems().ToList();

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
