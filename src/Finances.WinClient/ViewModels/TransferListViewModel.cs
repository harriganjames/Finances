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
    public interface ITransferListViewModel : IListViewModelBase<ITransferItemViewModel>
    {
        ActionCommand AddCommand { get; set; }
        ActionCommand EditCommand { get; set; }
        ActionCommand DeleteCommand { get; set; }
        ActionCommand ReloadCommand { get; set; }
        ObservableCollection<ITransferItemViewModel> Transfers { get; }
    }

    public class TransferListViewModel : ListViewModelBase<ITransferItemViewModel>, ITransferListViewModel
    {
        readonly ITransferService transferService;
        readonly IDialogService dialogService;
        readonly ITransferEditorViewModel transferEditorViewModel;

        public TransferListViewModel(ITransferService bankService,
                        IDialogService dialogService)
                        //ITransferEditorViewModel bankEditorViewModel)
        {
            this.transferService = bankService;
            this.dialogService = dialogService;
            //this.transferEditorViewModel = bankEditorViewModel;

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

        public ObservableCollection<ITransferItemViewModel> Transfers
        {
            get
            {
                return base.DataList;
            }
        }

        #endregion


        public override void WorkspaceOpened()
        {
            LoadData();
        }

        public override void WorkspaceClosed()
        {
            base.DataList.Clear();
        }

        private void Reload()
        {
            LoadData();
        }

        private void LoadData()
        {
            List<ITransferItemViewModel> transfers = null;

            base.IsBusy = true;

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) =>
            {
                transfers = this.transferService.ReadList();
            };
            bw.RunWorkerCompleted += (s, e) =>
            {
                base.DataList.Clear();
                if (transfers != null)
                {
                    transfers.ForEach(b =>
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
            //this.transferEditorViewModel.InitializeForAddEdit(true);

            //while (this.dialogService.ShowDialogView(this.transferEditorViewModel))
            //{
            //    bool result = this.transferService.Add(this.transferEditorViewModel);

            //    if (result)
            //    {
            //        ITransferItemViewModel newvm = this.transferService.CreateTransferViewItemModel();
            //        this.transferService.Read(this.transferEditorViewModel.BankId, newvm);
            //        base.DataList.Add(newvm);
            //        base.DataList.ToList().ForEach(i => i.IsSelected = false);
            //        newvm.IsSelected = true;
            //        break;
            //    }
            //}

        }


        private bool CanEdit()
        {
            return base.GetQtySelected() == 1;
        }
        private void Edit()
        {
            //ITransferItemViewModel vm = base.GetSelectedItems().First();

            //this.transferEditorViewModel.InitializeForAddEdit(false);

            //this.transferService.Read(vm.TransferId, this.transferEditorViewModel);

            //while (this.dialogService.ShowDialogView(this.transferEditorViewModel))
            //{
            //    bool result = this.transferService.Update(this.transferEditorViewModel);
            //    if (result)
            //    {
            //        this.transferService.Read(vm.TransferId, vm);
            //        break;
            //    }
            //}


        }


        private bool CanDelete()
        {
            return base.GetQtySelected() > 0;
        }
        private void Delete()
        {
            string title;
            string message;
            List<ITransferItemViewModel> sel = base.GetSelectedItems().ToList();

            if (sel.Count() == 0)
            {
                throw new Exception("Delete Transfer: nothing selected");
            }

            if (sel.Count() == 1)
            {
                title = "Delete Transfer";
                message = String.Format("Please confirm deletion of transfer: {0}", sel.First().Name);
            }
            else
            {
                title = "Delete Transfers";
                message = String.Format("Please confirm deletion of {0} trransfers?", sel.Count());
            }


            if (this.dialogService.ShowMessageBox(title, message, MessageBoxButtonEnum.YesNo) == MessageBoxResultEnum.Yes)
            {
                base.IsBusy = true;

                foreach (var b in sel)
                {
                    if (this.transferService.Delete(b))
                    {
                        base.DataList.Remove(b);
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
                return "Transfers";
            }
        }


    }
}
