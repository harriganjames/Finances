using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using AutoMapper;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Core.Wpf;
using Finances.WinClient.DomainServices;
using Finances.WinClient.Factories;

namespace Finances.WinClient.ViewModels
{
    //public interface IBankListViewModel 
    //{
    //    void Open();
    //    void Close();
    //}

    public class BankListViewModel : SortedListViewModelBase<BankItemViewModel>//, IBankListViewModel
    {
        readonly IBankRepository bankRepository;
        readonly IBankAgent bankAgent;

        // internal/parallel list of entiities
        Dictionary<int, Bank> banks = new Dictionary<int, Bank>();

        public BankListViewModel(
                        IBankRepository bankRepository,
                        IBankAgent bankAgent)
        {
            this.bankRepository = bankRepository;
            this.bankAgent = bankAgent;

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


        //public ObservableCollection<BankItemViewModel> Banks
        public ListCollectionView Banks
        {
            get
            {
                //if (this.dataListView == null)
                //{
                //    this.dataListView = new CollectionViewSource();
                //    this.dataListView.Source = base.DataList;
                //}
                //return (ListCollectionView)this.dataListView.View;
                //return base.DataList;
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


        public void Open()
        {
            LoadBanks();
        }

        public void Close()
        {
            base.DataList.Clear();
        }


        private void Reload()
        {
            LoadBanks();
        }

        private void LoadBanks()
        {
            //List<BankItemViewModel> banks = null;

            base.IsBusy = true;

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) =>
                {
                    //banks = this.bankService.ReadList();
                    //System.Threading.Thread.Sleep(5000);

                    banks.Clear();

                    var ebanks = this.bankRepository.ReadList();

                    if (ebanks != null)
                        ebanks.ForEach(eb => banks.Add(eb.BankId, eb));

                    //mapper.Map<List<BankItemViewModel>>(ebanks);

                };
            bw.RunWorkerCompleted += (s, e) =>
                {
                    base.DataList.Clear();
                    if (banks != null)
                    {
                        banks.Values.ToList().ForEach(b =>
                        {
                            base.DataList.Add(new BankItemViewModel(b));
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
            int id = this.bankAgent.Add();
            if (id > 0)
            {
                Bank entity = this.bankRepository.Read(id);

                this.banks.Add(entity.BankId, entity);

                var vm = new BankItemViewModel(entity);

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
            BankItemViewModel vm = base.GetSelectedItems().First();

            int id = vm.BankId;

            bool result = this.bankAgent.Edit(id);

            if (result)
            {
                this.banks[id] = this.bankRepository.Read(id);
                
                vm.Entity = this.banks[id];
            }

        }



        private bool CanDelete()
        {
            return base.GetQtySelected() > 0;
        }
        private void Delete()
        {
            var vms = base.GetSelectedItems().ToList();
            List<int> ids = vms.Select(vm => vm.BankId).ToList();

            if (this.bankAgent.Delete(ids))
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
                return "Banks";
            }
        }


    }
}
