using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
using System.Windows.Data;
using System.Threading.Tasks.Dataflow;
using Finances.Core;

namespace Finances.WinClient.ViewModels
{

    public class BalanceDateListViewModel : SortedListViewModelBase<BalanceDateItemViewModel>
    {
        readonly IBalanceDateRepository balanceDateRepository;
        readonly IBalanceDateAgent balanceDateAgent;

        ConcurrentDictionary<int, BalanceDate> balanceDates = new ConcurrentDictionary<int, BalanceDate>();

        


        public BalanceDateListViewModel(
                        IBalanceDateRepository balanceDateRepository,
                        IBalanceDateAgent balanceDateAgent
                        )
        {
            this.balanceDateRepository = balanceDateRepository;
            this.balanceDateAgent = balanceDateAgent;

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

        public ListCollectionView BalanceDates
        {
            get
            {
                return base.DataListView;
            }
        }

        bool adjustColumnWidthsSignal;
        public bool AdjustColumnWidthsSignal
        {
            get
            {
                return adjustColumnWidthsSignal;
            }
            set
            {
                adjustColumnWidthsSignal = value;
                NotifyPropertyChanged();
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


        public async void Open()
        {
            Diag.ThreadPrint("Open start");
            await LoadBalanceDates();
            Diag.ThreadPrint("Open end");
        }

        public void Close()
        {
            base.DataList.Clear();
        }

        private async void Reload()
        {
            Diag.ThreadPrint("Reload start");
            await LoadBalanceDates();
            Diag.ThreadPrint("Reload end");
        }



        private async Task LoadBalanceDates()
        {
            Diag.ThreadPrint("Load start");
            base.IsBusy = true;

            var target = new ActionBlock<BalanceDate>(a =>
            {
                balanceDates.GetOrAdd(a.BalanceDateId, a);
                base.DataList.Add(new BalanceDateItemViewModel(a));
                //Diag.ThreadPrint("In target. Account={0}", a.Name);

            }, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 1 });

            balanceDates.Clear();
            base.DataList.Clear();
            Diag.ThreadPrint("Load - before repo");
            await this.balanceDateRepository.PostList(target).ConfigureAwait(false);
            Diag.ThreadPrint("Load - after repo. qty={0}", balanceDates.Count);
            await target.Completion.ConfigureAwait(false);
            Diag.ThreadPrint("Load - after target completion. qty={0}", balanceDates.Count);

            base.IsBusy = false;

            AdjustColumnWidthsSignal = true;

            Diag.ThreadPrint("Load end");
        }


        private async Task LoadBalanceDates_old2()
        {
            var ts = TaskScheduler.FromCurrentSynchronizationContext();

            Diag.ThreadPrint("Load start");
            base.IsBusy = true;

            var tf = new TaskFactory(ts);

            var target = new ActionBlock<BalanceDate>(async a =>
            {
                await tf.StartNew(() =>
                {
                    balanceDates.GetOrAdd(a.BalanceDateId, a);
                    base.DataList.Add(new BalanceDateItemViewModel(a));
                    //Diag.ThreadPrint("In target. Account={0}", a.Name);
                });
            }, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 1 });

            balanceDates.Clear();
            base.DataList.Clear();

            Diag.ThreadPrint("Load - before repo");
            await this.balanceDateRepository.PostList(target);
            await target.Completion;

            Diag.ThreadPrint("Load - after repo. qty={0}", balanceDates.Count);

            base.IsBusy = false;
            Diag.ThreadPrint("Load end");
        }


        private void Add()
        {
            int id = this.balanceDateAgent.Add();
            if (id > 0)
            {
                var entity = this.balanceDateRepository.Read(id);

                this.balanceDates.GetOrAdd(entity.BalanceDateId, entity);

                var vm = new BalanceDateItemViewModel(entity);

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

            int id = vm.BalanceDateId;

            bool result = this.balanceDateAgent.Edit(id);

            if (result)
            {
                this.balanceDates[id] = this.balanceDateRepository.Read(id);

                vm.Entity = this.balanceDates[id];
            }

        }


        private bool CanDelete()
        {
            return base.GetQtySelected() > 0;
        }
        private void Delete()
        {
            var vms = base.GetSelectedItems().ToList();
            List<int> ids = vms.Select(vm => vm.BalanceDateId).ToList();

            if (this.balanceDateAgent.Delete(ids))
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
                return "Balance Dates";
            }
        }

    }

}
