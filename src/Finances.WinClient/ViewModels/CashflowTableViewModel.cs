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
        readonly ICashflowRepository cashflowRepository;
        readonly IEnumerable<ICashflowProjectionMode> aggregatedProjectionItemsGenerators;

        ObservableCollectionSafe<CashflowProjectionItem> cashflowProjectionItems;

        CashflowProjectionGroup cashflowProjectionGroup;

        public CashflowTableViewModel(ICashflowRepository cashflowRepository,
                                        IEnumerable<ICashflowProjectionMode> aggregatedProjectionItemsGenerators
                                        )
        {
            this.cashflowRepository = cashflowRepository;
            this.aggregatedProjectionItemsGenerators = aggregatedProjectionItemsGenerators;

            RefreshCommand = base.AddNewCommand(new ActionCommand(Refresh));
            ToggleModeCommand = base.AddNewCommand(new ActionCommand(ToggleMode));
        }


        #region Commmands

        public ActionCommand RefreshCommand { get; set; }
        public ActionCommand ToggleModeCommand { get; set; }

        #endregion


        public override void WorkspaceOpened()
        {
            LoadData();
        }

        public override void WorkspaceClosed()
        {
        }


        #region Bound Properties

        ObservableCollectionSafe<CashflowItemViewModel> cashflows;
        public ObservableCollectionSafe<CashflowItemViewModel> Cashflows
        {
            get
            {
                if (this.cashflows == null)
                    this.cashflows = new ObservableCollectionSafe<CashflowItemViewModel>();
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


        DateTime startDate;
        public DateTime? StartDate
        {
            get
            {
                return startDate == DateTime.MinValue ? DateTime.Now.Date.AddDays(1) : startDate;
            }
            set
            {
                startDate = value.HasValue ? value.Value : DateTime.MinValue;
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


        public ObservableCollectionSafe<CashflowProjectionItem> CashflowProjectionItems
        {
            get
            {
                if (cashflowProjectionItems == null)
                    cashflowProjectionItems = new ObservableCollectionSafe<CashflowProjectionItem>();
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


        #region LoadData

        private void LoadData()
        {
            base.IsBusy = true;

            Task<List<Cashflow>>.Factory
                .StartNew(() => RetrieveCashflowsTask())
                .ContinueWith(t => PopulateCashflowsTask(t.Result), base.UIScheduler)
                //.ContinueWith(t => GenerateProjection())
                .ContinueWith(t => base.IsBusy = false);

        }


        private List<Cashflow> RetrieveCashflowsTask()
        {
            return cashflowRepository.ReadList();
        }

        private void PopulateCashflowsTask(List<Cashflow> data)
        {
            int prevId = 0;
            if (SelectedCashflow != null)
                prevId = SelectedCashflow.CashflowId;

            Cashflows.Clear();
            SelectedCashflow = null;
            if (data != null)
            {
                data.ForEach(d => Cashflows.Add(new CashflowItemViewModel(d)));
                if (Cashflows.Count > 0)
                {
                    SelectedCashflow = prevId == 0 ? Cashflows[0] : Cashflows.FirstOrDefault(c => c.CashflowId == prevId);
                    NotifyPropertyChanged(() => this.SelectedCashflow);
                }
            }
        }

        #endregion


        private void Refresh()
        {
            GenerateProjection();
        }



        private async void GenerateProjection()
        {
            base.IsBusy = true;

            await RetreiveCashflowProjectionGroupAsync();

            await PopulateProjectionsFromGroupAsync();

            base.IsBusy = false;
        }



        private async Task RetreiveCashflowProjectionGroupAsync()
        {
            this.cashflowProjectionGroup = await SelectedCashflow.Entity.GenerateProjectionAsync(
                                    StartDate.Value,
                                    Convert.ToInt32(QtyMonths.Value),
                                    OpeningBalance.Value,
                                    Threshold.Value
                                    );

            //await Task.Factory.StartNew(() => {
            //    this.cashflowProjectionGroup = SelectedCashflow.Entity.GenerateProjectionAsync(
            //                            StartDate.Value,
            //                            Convert.ToInt32(QtyMonths.Value),
            //                            OpeningBalance.Value,
            //                            Threshold.Value
            //                            );
            //});
        }

        private async Task PopulateProjectionsFromGroupAsync()
        {
            await Task.Factory.StartNew(() => {
                CashflowProjectionItems.Clear();
                if (this.cashflowProjectionGroup != null && this.cashflowProjectionGroup.Items != null)
                {
                    this.cashflowProjectionGroup.DefaultItems.ToList().ForEach(cpi => CashflowProjectionItems.Add(cpi));
                }
            });
        }




        private void ToggleMode()
        {
            string periodGroup = selectedCashflowProjectionItem.PeriodGroup;

            if (periodGroup == null)
                return;

            // remember items belonging to period group
            var toggleItems = new List<CashflowProjectionItem>();

            int i = 0, startingIndex = -1;
            foreach (var item in CashflowProjectionItems)
            {
                if (item.PeriodGroup == periodGroup)
                {
                    toggleItems.Add(item);
                    if (startingIndex <= 0)
                        startingIndex = i;
                }
                i++;
            }

            // remove old items
            foreach (var item in toggleItems)
            {
                CashflowProjectionItems.Remove(item);
            }

            //toggle mode
            this.cashflowProjectionGroup.SwitchMode(periodGroup);

            // add new items
            foreach (var item in this.cashflowProjectionGroup.GetActiveItems(periodGroup))
            {
                CashflowProjectionItems.Insert(startingIndex++, item);
                if (SelectedCashflowProjectionItem == null)
                    SelectedCashflowProjectionItem = item;
            }

        }


        public override string Caption
        {
            get
            {
                return "Projections";
            }
        }

    }

}
