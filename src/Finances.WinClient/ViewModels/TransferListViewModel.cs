using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
    public interface ITransferListViewModel //: IListViewModelBase<TransferItemViewModel>
    {
    //    ActionCommand AddCommand { get; set; }
    //    ActionCommand EditCommand { get; set; }
    //    ActionCommand DeleteCommand { get; set; }
    //    ActionCommand ReloadCommand { get; set; }
    //    ObservableCollection<TransferItemViewModel> Transfers { get; }
        void Open();
        void Close();
    }

    public class TransferListViewModel : ListViewModelBase<TransferItemViewModel>, ITransferListViewModel
    {
        readonly ITransferRepository transferRepository;
        readonly IMappingEngine mapper;
        readonly IDialogService dialogService;
        readonly ITransferEditorViewModelFactory transferEditorViewModelFactory;

        Dictionary<int, Transfer> transfers = new Dictionary<int, Transfer>();

        public TransferListViewModel(
                        ITransferRepository transferRepository,
                        IMappingEngine mapper,
                        IDialogService dialogService,
                        ITransferEditorViewModelFactory bankEditorViewModelFactory
                        )
        {
            this.transferRepository = transferRepository;
            this.mapper = mapper;
            this.dialogService = dialogService;
            this.transferEditorViewModelFactory = bankEditorViewModelFactory;

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

        public ObservableCollection<TransferItemViewModel> Transfers
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
            LoadData();
        }

        public void Close()
        {
            base.DataList.Clear();
        }

        private void Reload()
        {
            LoadData();
        }

        private void LoadData()
        {
            //List<TransferItemViewModel> transfers = null;

            base.IsBusy = true;

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) =>
            {
                transfers.Clear();

                var entities = this.transferRepository.ReadList();

                if (entities != null)
                    entities.ForEach(t => transfers.Add(t.TransferId, t));

            };
            bw.RunWorkerCompleted += (s, e) =>
            {
                base.DataList.Clear();
                if (transfers != null)
                {
                    transfers.Values.ToList().ForEach(t =>
                    {
                        base.DataList.Add(mapper.Map<TransferItemViewModel>(t));
                    });
                }

                base.IsBusy = false;
            };

            bw.RunWorkerAsync();

        }



        private void Add()
        {
            var editor = this.transferEditorViewModelFactory.Create();

            editor.InitializeForAddEdit(true);

            while (this.dialogService.ShowDialogView(editor))
            {
                var entity = mapper.Map<Transfer>(editor);

                bool result = this.transferRepository.Add(entity) > 0;

                if (result)
                {
                    this.transfers.Add(entity.TransferId, entity);

                    var vm = mapper.Map<TransferItemViewModel>(entity);

                    base.DataList.Add(vm);
                    base.DataList.ToList().ForEach(i => i.IsSelected = false);
                    vm.IsSelected = true;
                    break;
                }
            }

            this.transferEditorViewModelFactory.Release(editor);
        }


        private bool CanEdit()
        {
            return base.GetQtySelected() == 1;
        }
        private void Edit()
        {
            var editor = this.transferEditorViewModelFactory.Create();

            TransferItemViewModel vm = base.GetSelectedItems().First();

            Transfer transfer = this.transfers[vm.TransferId];

            mapper.Map<Transfer, TransferEditorViewModel>(transfer, editor as TransferEditorViewModel);

            editor.InitializeForAddEdit(false);

            while (this.dialogService.ShowDialogView(editor))
            {
                var upd = mapper.Map<Transfer>(editor);

                bool result = this.transferRepository.Update(upd);
                if (result)
                {
                    this.transfers[vm.TransferId] = upd;
                    mapper.Map<Transfer, TransferItemViewModel>(upd, vm as TransferItemViewModel);
                    break;
                }
            }

            this.transferEditorViewModelFactory.Release(editor);

        }


        private bool CanDelete()
        {
            return base.GetQtySelected() > 0;
        }
        private void Delete()
        {
            string title;
            string message;
            List<TransferItemViewModel> sel = base.GetSelectedItems().ToList();

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

                foreach (var vm in sel)
                {
                    Transfer entity = mapper.Map<Transfer>(vm);
                    if (this.transferRepository.Delete(entity))
                    {
                        base.DataList.Remove(vm);
                        this.transfers.Remove(entity.TransferId);
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
