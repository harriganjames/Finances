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
using System.Windows.Data;

namespace Finances.WinClient.ViewModels
{
    public interface ITransferListViewModel
    {
        void Open();
        void Close();
    }

    public class TransferListViewModel : SortedListViewModelBase<TransferItemViewModel>, ITransferListViewModel
    {
        readonly ITransferRepository transferRepository;
        readonly ITransferAgent transferAgent;

        Dictionary<int, Transfer> transfers = new Dictionary<int, Transfer>();

        public TransferListViewModel(
                        ITransferRepository transferRepository,
                        ITransferAgent transferAgent
                        )
        {
            this.transferRepository = transferRepository;
            this.transferAgent = transferAgent;

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

        public ListCollectionView Transfers
        {
            get
            {
                return base.DataListView;
            }
        }


        decimal amountSum;
        public decimal AmountSum
        {
            get
            {
                return amountSum;
            }
            set
            {
                amountSum = value;
                NotifyPropertyChanged(() => this.AmountSum);
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
            transfers.Clear();
            base.DataList.Clear();
        }

        private void Reload()
        {
            LoadData();
        }

        private async void LoadData()
        {
            base.IsBusy = true;

            var task = new Task(() =>
            {
                transfers.Clear();

                var entities = this.transferRepository.ReadList();

                if (entities != null)
                    entities.ForEach(t => transfers.Add(t.TransferId, t));
            });

            task.Start();

            await task;

            base.DataList.Clear();
            if (transfers != null)
            {
                transfers.Values.ToList().ForEach(t =>
                {
                    base.DataList.Add(new TransferItemViewModel(t));
                });
            }

            base.IsBusy = false;
        }

        private void LoadData_old()
        {
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
                        base.DataList.Add(new TransferItemViewModel(t));
                    });
                }

                base.IsBusy = false;
            };

            bw.RunWorkerAsync();

        }


        private void Add()
        {
            int id = this.transferAgent.Add();
            if (id > 0)
            {
                var entity = this.transferRepository.Read(id);

                this.transfers.Add(entity.TransferId, entity);

                var vm = new TransferItemViewModel(entity);

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

            int id = vm.TransferId;

            bool result = this.transferAgent.Edit(id);

            if (result)
            {
                this.transfers[id] = this.transferRepository.Read(id);

                vm.Entity = this.transfers[id];
            }

        }



        private bool CanDelete()
        {
            return base.GetQtySelected() > 0;
        }

        private void Delete()
        {
            var vms = base.GetSelectedItems().ToList();
            List<int> ids = vms.Select(vm => vm.TransferId).ToList();

            if (this.transferAgent.Delete(ids))
            {
                base.IsBusy = true;

                foreach (var vm in vms)
                {
                    base.DataList.Remove(vm);
                }

                base.IsBusy = false;

            }

        }


        protected override void ItemSelectionChanged()
        {
            base.ItemSelectionChanged();
            AggregateAmount();

        }


        void AggregateAmount()
        {
            //decimal amount = 0;
            //this.DataList.ToList().ForEach(t=>amount+=t.IsSelected?t.Amount:0);
            this.AmountSum = this.DataList.Where(t=>t.IsSelected).Sum(t=>t.Amount);
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
