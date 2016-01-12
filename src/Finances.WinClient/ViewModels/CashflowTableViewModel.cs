using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.Wpf;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.WinClient.DomainServices;
using Finances.WinClient.Factories;
using System.Threading;

using Finances.Core.Engines.Cashflow;
using Finances.Core.Engines;
using Finances.Core.ValueObjects;
using System.Collections.ObjectModel;


//
// 
//
//
//

namespace Finances.WinClient.ViewModels
{
    public class CashflowTableViewModel : Workspace
    {
        readonly IRepositoryRead<Cashflow> cashflowRepositoryRead;
        //readonly ICashflowEngineA cashflowEngineA;
        //readonly ICashflowEngineFactoryB cashflowEngineFactoryB;
        //readonly ICashflowEngineC cashflowEngineC;
        readonly IEnumerable<ICashflowProjectionMode> aggregatedProjectionItemsGenerators;

        ObservableCollection<CashflowProjectionItem> cashflowProjectionItems;


        public CashflowTableViewModel(IRepositoryRead<Cashflow> cashflowRepositoryRead,
                                        //ICashflowEngineA cashflowEngineA,
                                        //ICashflowEngineFactoryB cashflowEngineFactoryB,
                                        //ICashflowEngineC cashflowEngineC,
                                        IEnumerable<ICashflowProjectionMode> aggregatedProjectionItemsGenerators
                                        )
        {
            this.cashflowRepositoryRead = cashflowRepositoryRead;
            //this.cashflowEngineA = cashflowEngineA;
            //this.cashflowEngineFactoryB = cashflowEngineFactoryB;
            //this.cashflowEngineC = cashflowEngineC;
            this.aggregatedProjectionItemsGenerators = aggregatedProjectionItemsGenerators;

            RefreshCommand = base.AddNewCommand(new ActionCommand(Refresh));
        }


        #region Commmands

        public ActionCommand RefreshCommand { get; set; }

        #endregion


        public override void WorkspaceOpened()
        {
            LoadData();
        }

        public override void WorkspaceClosed()
        {
        }

        #region Bound Properties

        ObservableCollection<CashflowItemViewModel> cashflows;
        public ObservableCollection<CashflowItemViewModel> Cashflows
        {
            get
            {
                if (this.cashflows == null)
                    this.cashflows = new ObservableCollection<CashflowItemViewModel>();
                return this.cashflows;
            }
        }

        CashflowItemViewModel selectedCashflow;
        public CashflowItemViewModel SelectedCashflow
        {
            get { return selectedCashflow; }
            set 
            {
                //if (selectedCashflow != null && value != null && selectedCashflow.CashflowId == value.CashflowId)
                if (selectedCashflow != value)
                {
                    selectedCashflow = value;
                    OpeningBalance.Value = selectedCashflow==null ? 0M : selectedCashflow.OpeningBalance;
                    //NotifyPropertyChanged(() => this.SelectedCashflow);
                    GenerateProjection();
                }
            }
        }

        InputDecimal qtyMonths;
        public InputDecimal QtyMonths
        {
            get
            {
                if (qtyMonths == null)
                {
                    qtyMonths = new InputDecimal()
                    {
                        FormatString = "n0",
                        Mandatory = true,
                        AllowInvalid = false,
                        Value = 6
                    };
                }
                return qtyMonths;
            }
        }

        InputDecimal openingBalance;
        public InputDecimal OpeningBalance
        {
            get
            {
                if (openingBalance == null)
                {
                    openingBalance = new InputDecimal()
                    {
                        FormatString = "c",
                        Mandatory = true,
                        AllowInvalid = false,
                        Value = 0
                    };
                }
                return openingBalance;
            }
        }

        InputDecimal threshold;
        public InputDecimal Threshold
        {
            get
            {
                if (threshold == null)
                {
                    threshold = new InputDecimal()
                    {
                        FormatString = "c",
                        Mandatory = true,
                        AllowInvalid = false,
                        Value = 0
                    };
                }
                return threshold;
            }
        }

        List<ICashflowProjectionMode> modes;
        public List<ICashflowProjectionMode> Modes
        {
            get
            {
                if (modes == null)
                {
                    modes = new List<ICashflowProjectionMode>(); 
                    foreach (var m in this.aggregatedProjectionItemsGenerators)
                    {
                        modes.Add(m);
                    }
                    //SelectedMode = modes.FirstOrDefault();
                }
                return modes;
            }
        }

        ICashflowProjectionMode selectedMode;
        public ICashflowProjectionMode SelectedMode
        {
            get
            {
                if (selectedMode == null)
                    selectedMode = Modes.FirstOrDefault();
                return selectedMode;
            }
            set
            {
                if (selectedMode != value)
                {
                    selectedMode = value;
                    NotifyPropertyChanged(() => SelectedMode);
                }
            }
        }


        public ObservableCollection<CashflowProjectionItem> CashflowProjectionItems
        {
            get
            {
                if (cashflowProjectionItems == null)
                    cashflowProjectionItems = new ObservableCollection<CashflowProjectionItem>();
                return cashflowProjectionItems;
            }
            //set 
            //{ 
            //    cashflowProjectionItems = value;
            //    NotifyPropertyChanged(() => CashflowProjectionItems);
            //}
        }

        private CashflowProjectionItem selectedCashflowProjectionItem;
        public CashflowProjectionItem SelectedCashflowProjectionItem
        {
            get
            {
                return selectedCashflowProjectionItem;
            }

            set
            {
                selectedCashflowProjectionItem = value;
                NotifyPropertyChanged(() => this.SelectedCashflowProjectionItem);
            }
        }



        #endregion

        //BalanceState property? negative, below threshold, ok



        private void Refresh()
        {
            GenerateProjection();
            //GenerateProjectionTask();
        }


        private void LoadData()
        {
            base.IsBusy = true;

            Task<List<Cashflow>>.Factory
                .StartNew(() => RetrieveCashflowsTask())
                .ContinueWith(t => PopulateCashflowsTask(t.Result), base.UIScheduler)
                //.ContinueWith(t => GenerateProjection())
                .ContinueWith(t => base.IsBusy = false);

        }

        private void GenerateProjection()
        {
            base.IsBusy = true;

            Task.Factory
                .StartNew(() => RetreiveProjectionsTask())
                .ContinueWith(t => PopulateProjectionsTask(t.Result), base.UIScheduler)
                //.StartNew(() => GenerateProjectionTask())
                .ContinueWith(t => base.IsBusy = false);
        }



        private List<Cashflow> RetrieveCashflowsTask()
        {
            return cashflowRepositoryRead.ReadList();
        }

        private void PopulateCashflowsTask(List<Cashflow> data)
        {
            int prevId = 0;
            if (SelectedCashflow != null)
                prevId = SelectedCashflow.CashflowId;

            Cashflows.Clear();
            SelectedCashflow = null;
            data.ForEach(d => Cashflows.Add(new CashflowItemViewModel(d)));
            if (Cashflows.Count > 0)
            {
                SelectedCashflow = prevId == 0 ? Cashflows[0] : Cashflows.FirstOrDefault(c => c.CashflowId == prevId);
                NotifyPropertyChanged(() => this.SelectedCashflow);
            }
        }

        private List<CashflowProjectionItem> RetreiveProjectionsTask()
        {
            return SelectedCashflow.Entity.GenerateProjection(
                                    SelectedCashflow.Entity.StartDate.AddMonths(Decimal.ToInt32(QtyMonths.Value)),
                                    OpeningBalance.Value,
                                    Threshold.Value,
                                    SelectedMode);
        }

        private void PopulateProjectionsTask(List<CashflowProjectionItem> items)
        {
            CashflowProjectionItems.Clear();
            if (items != null)
            {
                items.ForEach(cpi => CashflowProjectionItems.Add(cpi));
            }
            this.SelectedCashflowProjectionItem = CashflowProjectionItems.FirstOrDefault();
        }


        //private void GenerateProjectionTask()
        //{
        //    if (SelectedCashflow != null)
        //    {
        //        List<CashflowProjectionItem> items = SelectedCashflow.Entity.GenerateProjection(
        //                            SelectedCashflow.Entity.StartDate.AddMonths(Decimal.ToInt32(QtyMonths.Value)),
        //                            OpeningBalance.Value, 
        //                            Threshold.Value, 
        //                            SelectedMode);

        //        CashflowProjectionItems.Clear();
        //        if (items!=null)
        //        {
        //            items.ForEach(cpi => CashflowProjectionItems.Add(cpi));
        //        }

        //    }
        //    else
        //        CashflowProjectionItems.Clear();
        //}

    }

}
