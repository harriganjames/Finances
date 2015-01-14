using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.Wpf;
using Finances.WinClient.DomainServices;

namespace Finances.WinClient.ViewModels
{
    public interface IBankAccountsViewModel : IListViewModelBase<IBankAccountItemViewModel>
    {
        ActionCommand AddCommand { get; set; }
        ActionCommand EditCommand { get; set; }
        ActionCommand DeleteCommand { get; set; }
        ActionCommand ReloadCommand { get; set; }
        ObservableCollection<IBankAccountItemViewModel> BankAccounts { get; }
    }

    public class BankAccountsViewModel : ListViewModelBase<IBankAccountItemViewModel>, IBankAccountsViewModel
    {
        readonly IBankAccountService bankAccountService;
        readonly IDialogService dialogService;
        readonly IBankAccountEditorViewModel bankAccountEditorViewModel;

        public BankAccountsViewModel(IBankAccountService bankService,
                        IDialogService dialogService,
                        IBankAccountEditorViewModel bankAccountEditorViewModel)
        {
            this.bankAccountService = bankService;
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
            List<IBankAccountItemViewModel> banks = null;

            base.IsBusy = true;

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) =>
            {
                banks = this.bankAccountService.ReadList();
            };
            bw.RunWorkerCompleted += (s, e) =>
            {
                base.DataList.Clear();
                if (banks != null)
                {
                    banks.ForEach(b =>
                    {
                        base.DataList.Add(b);
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
                bool result = this.bankAccountService.Add(this.bankAccountEditorViewModel);

                if (result)
                {
                    //IBankAccountItemViewModel newvm = this.bankAccountService.CreateBankAccountItemViewModel();
                    IBankAccountItemViewModel newvm = new BankAccountItemViewModel();
                    this.bankAccountService.Read(this.bankAccountEditorViewModel.BankAccountId, newvm);
                    base.DataList.Add(newvm);
                    base.DataList.ToList().ForEach(i => i.IsSelected = false);
                    newvm.IsSelected = true;
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

            this.bankAccountEditorViewModel.InitializeForAddEdit(false);

            this.bankAccountService.Read(vm.BankAccountId, this.bankAccountEditorViewModel);

            while (this.dialogService.ShowDialogView(this.bankAccountEditorViewModel))
            {
                bool result = this.bankAccountService.Update(this.bankAccountEditorViewModel);
                if (result)
                {
                    this.bankAccountService.Read(vm.BankAccountId, vm);
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

                foreach (var a in sel)
                {
                    if (this.bankAccountService.Delete(a))
                    {
                        base.DataList.Remove(a);
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
