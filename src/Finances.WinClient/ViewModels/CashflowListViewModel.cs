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
    public interface ICashflowListViewModel
    {
        void Open();
        void Close();
    }

    public class CashflowListViewModel : ListViewModelBase<CashflowItemViewModel>, ICashflowListViewModel
    {
        readonly ICashflowRepository cashflowRepository;
        //readonly IRepositoryRead<Cashflow> cashflowRepository;
        readonly ICashflowAgent cashflowAgent;

        Dictionary<int, Cashflow> cashflows = new Dictionary<int, Cashflow>();

        public CashflowListViewModel(
                        ICashflowRepository cashflowRepository,
                        ICashflowAgent cashflowAgent
                        )
        {
            this.cashflowRepository = cashflowRepository;
            this.cashflowAgent = cashflowAgent;

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

        public ObservableCollection<CashflowItemViewModel> Cashflows
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
            base.IsBusy = true;

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) =>
            {
                cashflows.Clear();

                var entities = this.cashflowRepository.ReadList();

                if (entities != null)
                    entities.ForEach(t => cashflows.Add(t.CashflowId, t));

            };
            bw.RunWorkerCompleted += (s, e) =>
            {
                base.DataList.Clear();
                if (cashflows != null)
                {
                    cashflows.Values.ToList().ForEach(t =>
                    {
                        base.DataList.Add(new CashflowItemViewModel(t));
                    });
                }

                base.IsBusy = false;
            };

            bw.RunWorkerAsync();

        }
        private void Add()
        {
            int id = this.cashflowAgent.Add();
            if (id > 0)
            {
                Cashflow entity = this.cashflowRepository.Read(id);

                this.cashflows.Add(entity.CashflowId, entity);

                var vm = new CashflowItemViewModel(entity);

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
            CashflowItemViewModel vm = base.GetSelectedItems().First();

            int id = vm.CashflowId;

            bool result = this.cashflowAgent.Edit(id);

            if (result)
            {
                this.cashflows[id] = this.cashflowRepository.Read(id);

                vm.Entity = this.cashflows[id];
            }

        }


        private bool CanDelete()
        {
            return base.GetQtySelected() > 0;
        }
        private void Delete()
        {
            var vms = base.GetSelectedItems().ToList();
            List<int> ids = vms.Select(vm => vm.CashflowId).ToList();

            if (this.cashflowAgent.Delete(ids))
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
                return "Cashflows";
            }
        }


    }
}
