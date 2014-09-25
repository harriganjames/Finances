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
    public interface IBanksViewModel : IListViewModelBase<IBankItemViewModel>
    {
        ActionCommand AddCommand { get; set; }
        ActionCommand EditCommand { get; set; }
        ActionCommand DeleteCommand { get; set; }
        ActionCommand ReloadCommand { get; set; }
        ObservableCollection<IBankItemViewModel> Banks { get; }
    }

    public class BanksViewModel : ListViewModelBase<IBankItemViewModel>, IBanksViewModel
    {
        readonly IBankService bankService;
        readonly IDialogService dialogService;
        readonly IBankEditorViewModel bankEditorViewModel;

        public BanksViewModel(IBankService bankService,
                        IDialogService dialogService,
                        IBankEditorViewModel bankEditorViewModel)
        {
            this.bankService = bankService;
            this.dialogService = dialogService;
            this.bankEditorViewModel = bankEditorViewModel;

            ReloadCommand = base.AddNewCommand(new ActionCommand(Reload));
            AddCommand = base.AddNewCommand(new ActionCommand(Add));
            EditCommand = base.AddNewCommand(new ActionCommand(Edit,CanEdit));
            DeleteCommand = base.AddNewCommand(new ActionCommand(Delete, CanDelete));
        }


        #region Commmands

        public ActionCommand ReloadCommand { get; set; }
        public ActionCommand AddCommand { get; set; }
        public ActionCommand EditCommand { get; set; }
        public ActionCommand DeleteCommand { get; set; }

        #endregion

        #region PublicProperties

        public ObservableCollection<IBankItemViewModel> Banks
        {
            get
            {
                return base.DataList;
            }
        }

        #endregion


        public override void WorkspaceOpened()
        {
            LoadBanks();
        }

        public override void WorkspaceClosed()
        {
            base.DataList.Clear();
        }

        private void Reload()
        {
            LoadBanks();
        }

        private void LoadBanks()
        {
            List<IBankItemViewModel> banks = null;

            base.IsBusy = true;

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) =>
                {
                    banks = this.bankService.ReadList();
                    //System.Threading.Thread.Sleep(5000);
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


            //await Task.Run(() =>
            //{
            //    banks = this.bankService.ReadList();
            //    System.Threading.Thread.Sleep(5000);
            //    int i = 0;

            //    base.DataList.Clear();
            //    if (banks != null)
            //    {
            //        banks.ForEach(b =>
            //        {
            //            base.DataList.Add(b);
            //        });
            //    }

            //});
                

            //base.IsBusy = false;

        }



        private void Add()
        {
            this.bankEditorViewModel.InitializeForAddEdit(true);

            while (this.dialogService.ShowDialogView(this.bankEditorViewModel))
            {
                bool result = this.bankService.Add(this.bankEditorViewModel);

                if (result)
                {
                    IBankItemViewModel newvm = this.bankService.CreateBankViewModel();
                    this.bankService.Read(this.bankEditorViewModel.BankId, newvm);
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
            IBankItemViewModel vm = base.GetSelectedItems().First();
            
            this.bankEditorViewModel.InitializeForAddEdit(false);

            this.bankService.Read(vm.BankId, this.bankEditorViewModel);

            while (this.dialogService.ShowDialogView(this.bankEditorViewModel))
            {
                bool result = this.bankService.Update(this.bankEditorViewModel);
                 if (result)
                {
                    this.bankService.Read(vm.BankId, vm);
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
            List<IBankItemViewModel> sel = base.GetSelectedItems().ToList();

            if (sel.Count() == 0)
            {
                throw new Exception("Delete Bank: nothing selected");
            }

            if (sel.Count() == 1)
            {
                title = "Delete Bank";
                message = String.Format("Please confirm deletion of bank: {0}", sel.First().Name);
            }
            else
            {
                title = "Delete Banks";
                message = String.Format("Please confirm deletion of {0} banks?", sel.Count());
            }


            if (this.dialogService.ShowMessageBox(title, message, MessageBoxButtonEnum.YesNo) == MessageBoxResultEnum.Yes)
            {
                base.IsBusy = true;

                foreach (var b in sel)
                {
                    if (this.bankService.Delete(b.BankId))
                    {
                        base.DataList.Remove(b);
                    }
                    else
                        break;
                }

                base.IsBusy = false;
            }
        }


        //public override string ToString()
        //{
        //        return "Hello";
        //}

    }
}
