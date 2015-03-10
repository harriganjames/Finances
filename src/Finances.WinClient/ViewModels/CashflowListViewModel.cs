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
        readonly IMappingEngine mapper;
        readonly IDialogService dialogService;
        readonly ICashflowEditorViewModelFactory cashflowEditorViewModelFactory;
        readonly ICashflowAgent cashflowAgent;

        Dictionary<int, Cashflow> cashflows = new Dictionary<int, Cashflow>();

        public CashflowListViewModel(
                        ICashflowRepository cashflowRepository,
                        IMappingEngine mapper,
                        IDialogService dialogService,
                        ICashflowEditorViewModelFactory bankEditorViewModelFactory,
                        ICashflowAgent cashflowAgent
                        )
        {
            this.cashflowRepository = cashflowRepository;
            this.mapper = mapper;
            this.dialogService = dialogService;
            this.cashflowEditorViewModelFactory = bankEditorViewModelFactory;
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
            //List<CashflowItemViewModel> cashflows = null;

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
                        base.DataList.Add(mapper.Map<CashflowItemViewModel>(t));
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

                var vm = mapper.Map<CashflowItemViewModel>(entity);

                base.DataList.Add(vm);
                base.DataList.ToList().ForEach(i => i.IsSelected = false);
                vm.IsSelected = true;

            }
        
        }

        private void AddOld()
        {
            var editor = this.cashflowEditorViewModelFactory.Create();

            var entity = new Cashflow();

            editor.InitializeForAddEdit(true,entity);

            while (this.dialogService.ShowDialogView(editor))
            {
                //var entity = mapper.Map<Cashflow>(editor);

                bool result = this.cashflowRepository.Add(entity) > 0;

                if (result)
                {
                    this.cashflows.Add(entity.CashflowId, entity);

                    var vm = mapper.Map<CashflowItemViewModel>(entity);

                    base.DataList.Add(vm);
                    base.DataList.ToList().ForEach(i => i.IsSelected = false);
                    vm.IsSelected = true;
                    break;
                }
            }

            this.cashflowEditorViewModelFactory.Release(editor);
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
                mapper.Map<Cashflow, CashflowItemViewModel>(this.cashflows[id], vm);
            }

        }

        //private void EditOld()
        //{
        //    var editor = this.cashflowEditorViewModelFactory.Create();

        //    CashflowItemViewModel vm = base.GetSelectedItems().First();

        //    Cashflow cashflow = this.cashflows[vm.CashflowId];

        //    Cashflow upd = mapper.Map<Cashflow>(cashflow);

        //    editor.InitializeForAddEdit(false,upd);

        //    while (this.dialogService.ShowDialogView(editor))
        //    {
        //        bool result = this.cashflowRepository.Update(upd);
        //        if (result)
        //        {
        //            this.cashflows[vm.CashflowId] = upd;
        //            mapper.Map<Cashflow, CashflowItemViewModel>(upd, vm as CashflowItemViewModel);
        //            break;
        //        }
        //    }

        //    this.cashflowEditorViewModelFactory.Release(editor);

        //}


        private bool CanDelete()
        {
            return base.GetQtySelected() > 0;
        }
        private void Delete()
        {
            string title;
            string message;
            List<CashflowItemViewModel> sel = base.GetSelectedItems().ToList();

            if (sel.Count() == 0)
            {
                throw new Exception("Delete Cashflow: nothing selected");
            }

            if (sel.Count() == 1)
            {
                title = "Delete Cashflow";
                message = String.Format("Please confirm deletion of cashflow: {0}", sel.First().Name);
            }
            else
            {
                title = "Delete Cashflows";
                message = String.Format("Please confirm deletion of {0} trransfers?", sel.Count());
            }


            if (this.dialogService.ShowMessageBox(title, message, MessageBoxButtonEnum.YesNo) == MessageBoxResultEnum.Yes)
            {
                base.IsBusy = true;

                foreach (var vm in sel)
                {
                    Cashflow entity = mapper.Map<Cashflow>(vm);
                    if (this.cashflowRepository.Delete(entity))
                    {
                        base.DataList.Remove(vm);
                        this.cashflows.Remove(entity.CashflowId);
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
                return "Cashflows";
            }
        }


    }
}
