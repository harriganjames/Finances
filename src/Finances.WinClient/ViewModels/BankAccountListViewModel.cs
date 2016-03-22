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
    //public interface IBankAccountListViewModel 
    //{
    //    void Open();
    //    void Close();
    //}

    public class BankAccountListViewModel : SortedListViewModelBase<BankAccountItemViewModel> //, IBankAccountListViewModel
    {
        readonly IBankAccountRepository bankAccountRepository;
        readonly IBankAccountAgent bankAccountAgent;

        ConcurrentDictionary<int, BankAccount> bankAccounts = new ConcurrentDictionary<int, BankAccount>();

        


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

        public ListCollectionView BankAccounts
        {
            get
            {
                return base.DataListView;
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
            await LoadBankAccounts();
            Diag.ThreadPrint("Open end");
        }

        public void Close()
        {
            base.DataList.Clear();
        }

        private async void Reload()
        {
            Diag.ThreadPrint("Reload start");
            await LoadBankAccounts();
            Diag.ThreadPrint("Reload end");
        }



        private async Task LoadBankAccounts()
        {
            Diag.ThreadPrint("Load start");
            base.IsBusy = true;

            var target = new ActionBlock<BankAccount>(a =>
            {
                bankAccounts.GetOrAdd(a.BankAccountId, a);
                base.DataList.Add(new BankAccountItemViewModel(a));
                //Diag.ThreadPrint("In target. Account={0}", a.Name);

            }, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 1 });

            bankAccounts.Clear();
            base.DataList.Clear();
            Diag.ThreadPrint("Load - before repo");
            await this.bankAccountRepository.PostList(target).ConfigureAwait(false);
            Diag.ThreadPrint("Load - after repo. qty={0}", bankAccounts.Count);
            await target.Completion.ConfigureAwait(false);
            Diag.ThreadPrint("Load - after target completion. qty={0}", bankAccounts.Count);

            base.IsBusy = false;
            Diag.ThreadPrint("Load end");
        }


        private async Task LoadBankAccounts_old2()
        {
            var ts = TaskScheduler.FromCurrentSynchronizationContext();

            Diag.ThreadPrint("Load start");
            base.IsBusy = true;

            var tf = new TaskFactory(ts);

            var target = new ActionBlock<BankAccount>(async a =>
            {
                await tf.StartNew(() =>
                {
                    bankAccounts.GetOrAdd(a.BankAccountId, a);
                    base.DataList.Add(new BankAccountItemViewModel(a));
                    Diag.ThreadPrint("In target. Account={0}", a.Name);
                });
            }, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 1 });

            bankAccounts.Clear();
            base.DataList.Clear();

            Diag.ThreadPrint("Load - before repo");
            await this.bankAccountRepository.PostList(target);
            await target.Completion;

            Diag.ThreadPrint("Load - after repo. qty={0}", bankAccounts.Count);

            base.IsBusy = false;
            Diag.ThreadPrint("Load end");
        }


        private async Task LoadBankAccounts_ok()
        {
            Diag.ThreadPrint("Load start");
            base.IsBusy = true;

            var target = new ActionBlock<BankAccount>(a =>
            {
                bankAccounts.GetOrAdd(a.BankAccountId, a);
                Diag.ThreadPrint("In target. Account={0}", a.Name);

            }, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism=4 });

            bankAccounts.Clear();
            Diag.ThreadPrint("Load - before repo");
            await this.bankAccountRepository.PostList(target);
            Diag.ThreadPrint("Load - after repo. qty={0}", bankAccounts.Count);

            base.DataList.Clear();
            Diag.ThreadPrint("Load - after clear");
            if (bankAccounts != null)
            {
                Diag.ThreadPrint("Load - before populate");
                bankAccounts.Values.ToList().ForEach(a =>
                {
                    base.DataList.Add(new BankAccountItemViewModel(a));
                });
                Diag.ThreadPrint("Load - after populate");
            }

            base.IsBusy = false;
            Diag.ThreadPrint("Load end");
        }

        //private void LoadBankAccounts_old()
        //{
        //    base.IsBusy = true;

        //    BackgroundWorker bw = new BackgroundWorker();
        //    bw.DoWork += (s, e) =>
        //    {
        //        bankAccounts.Clear();

        //        var entities = this.bankAccountRepository.ReadList();

        //        if (entities != null)
        //            entities.ForEach(a => bankAccounts.Add(a.BankAccountId, a));

        //    };
        //    bw.RunWorkerCompleted += (s, e) =>
        //    {
        //        base.DataList.Clear();
        //        if (bankAccounts != null)
        //        {
        //            bankAccounts.Values.ToList().ForEach(a =>
        //            {
        //                base.DataList.Add(new BankAccountItemViewModel(a));
        //            });
        //        }

        //        base.IsBusy = false;
        //    };

        //    bw.RunWorkerAsync();

        //}

        private void Add()
        {
            int id = this.bankAccountAgent.Add();
            if (id > 0)
            {
                var entity = this.bankAccountRepository.Read(id);

                this.bankAccounts.GetOrAdd(entity.BankAccountId, entity);

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
