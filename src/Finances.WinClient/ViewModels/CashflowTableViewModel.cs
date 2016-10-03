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
        readonly IBalanceDateRepository balanceDateRepository;

        ObservableCollectionSafe<CashflowProjectionItem> cashflowProjectionItems;

        CashflowProjectionGroup cashflowProjectionGroup;

        public CashflowTableViewModel(ICashflowRepository cashflowRepository,
                                        IEnumerable<ICashflowProjectionMode> aggregatedProjectionItemsGenerators,
                                        IBalanceDateRepository balanceDateRepository
                                        )
        {
            this.cashflowRepository = cashflowRepository;
            this.aggregatedProjectionItemsGenerators = aggregatedProjectionItemsGenerators;
            this.balanceDateRepository = balanceDateRepository;

            ReloadCommand = base.AddNewCommand(new ActionCommand(Reload));
            ToggleModeCommand = base.AddNewCommand(new ActionCommand(ToggleMode));
        }


        #region Commmands

        public ActionCommand ReloadCommand { get; set; }
        public ActionCommand ToggleModeCommand { get; set; }

        #endregion


        public override void WorkspaceOpened()
        {
            Reload();
            //LoadReferenceDataAsync();
            //GenerateProjectionAsync();
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
                    SelectedCashflowChanged();
                }
            }
        }


        ObservableCollectionSafe<BalanceDateItemViewModel> balanceDates;
        public ObservableCollectionSafe<BalanceDateItemViewModel> BalanceDates
        {
            get
            {
                if (this.balanceDates == null)
                    this.balanceDates = new ObservableCollectionSafe<BalanceDateItemViewModel>();
                return this.balanceDates;
            }
        }

        BalanceDateItemViewModel selectedBalanceDate;
        public BalanceDateItemViewModel SelectedBalanceDate
        {
            get { return selectedBalanceDate; }
            set
            {
                if (selectedBalanceDate != value)
                {
                    selectedBalanceDate = value;
                    SelectedBalanceDateChanged();
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
                        Value = 15
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

        bool isOpeningBalanceAvailable;
        public bool IsOpeningBalanceAvailable
        {
            get
            {
                return isOpeningBalanceAvailable;
            }

            set
            {
                isOpeningBalanceAvailable = value;
                NotifyPropertyChanged();
            }
        }

        #endregion


        bool isLoading = false;

        #region LoadRefernceData

        private async Task LoadReferenceDataAsync()
        {
            base.IsBusy = true;

            isLoading = true;

            //await Task<List<Cashflow>>.Factory
            //    .StartNew(() => RetrieveCashflowsTask())
            //    .ContinueWith(t => PopulateCashflowsTask(t.Result), base.UIScheduler);

            await PopulateCashflowsAsync();

            await PopulateBalanceDatesAsync();

            isLoading = false;

            base.IsBusy = false;
        }

        private async Task PopulateCashflowsAsync()
        {
            var cs = cashflowRepository.ReadList();

            this.SelectedCashflow = await PopulateListAsync(cs, Cashflows, SelectedCashflow, (d) => new CashflowItemViewModel(d));

            NotifyPropertyChanged(()=>this.SelectedCashflow);
        }

        private async Task PopulateBalanceDatesAsync()
        {
            var bds = await balanceDateRepository.ReadListAsync();

            this.SelectedBalanceDate = await PopulateListAsync(bds, BalanceDates, SelectedBalanceDate, (d) => new BalanceDateItemViewModel(d));
            NotifyPropertyChanged(() => this.SelectedBalanceDate);

        }

        private async Task<TViewModel> PopulateListAsync<TViewModel,TData>(List<TData> data, IList<TViewModel> list, TViewModel selected, Func<TData,TViewModel> factory) where TViewModel : ItemViewModelBase
        {
            return await Task.Factory.StartNew(() =>
            {
                TViewModel rv = selected;
                int prevId = 0;
                if (selected != null)
                    prevId = selected.Id;

                list.Clear();
                if (data != null)
                {
                    data.ForEach(d => list.Add(factory(d)));
                    if (list.Count > 0)
                    {
                        rv = prevId == 0 ? list[0] : list.FirstOrDefault(c => c.Id == prevId);
                    }
                }
                return rv;
            });
        }




        //private List<Cashflow> RetrieveCashflowsTask()
        //{
        //    return cashflowRepository.ReadList();
        //}

        //private void PopulateCashflowsTask(List<Cashflow> data)
        //{
        //    int prevId = 0;
        //    if (SelectedCashflow != null)
        //        prevId = SelectedCashflow.CashflowId;

        //    Cashflows.Clear();
        //    SelectedCashflow = null;
        //    if (data != null)
        //    {
        //        data.Where(i=>i.CashflowBankAccounts.Count>0).ToList().ForEach(d => Cashflows.Add(new CashflowItemViewModel(d)));
        //        if (Cashflows.Count > 0)
        //        {
        //            SelectedCashflow = prevId == 0 ? Cashflows[0] : Cashflows.FirstOrDefault(c => c.CashflowId == prevId);
        //            NotifyPropertyChanged(() => this.SelectedCashflow);
        //        }
        //    }

        //}

        #endregion


        private async void Reload()
        {
            await LoadReferenceDataAsync();
            await GenerateProjectionAsync();
        }

        private async void SelectedBalanceDateChanged()
        {
            CalculateOpeningBalance();
            await GenerateProjectionAsync();
        }

        private async void SelectedCashflowChanged()
        {
            CalculateOpeningBalance();
            await GenerateProjectionAsync();
        }

        private void CalculateOpeningBalance()
        {
            this.IsOpeningBalanceAvailable = false;

            if (SelectedCashflow == null || SelectedBalanceDate == null)
                return;

            decimal openingBalance = 0;
            int missing = 0;
            foreach (var ba in SelectedCashflow.Entity.CashflowBankAccounts)
            {
                var bda = SelectedBalanceDate.Entity.BalanceDateBankAccounts.FirstOrDefault(b => b.BankAccount.BankAccountId == ba.BankAccount.BankAccountId);
                if (bda != null)
                    openingBalance += bda.BalanceAmount;
                else
                    missing++;
            }

            if (missing == 0)
            {
                OpeningBalance.Value = openingBalance;
                IsOpeningBalanceAvailable = true;
            }
        }

        private async Task GenerateProjectionAsync()
        {
            if (isLoading)
                return;

            if (SelectedCashflow == null || SelectedBalanceDate == null)
                return;

            if(!IsOpeningBalanceAvailable)
            {
                CashflowProjectionItems.Clear();
                return;
            }

            base.IsBusy = true;

            await RetreiveCashflowProjectionGroupAsync();

            await PopulateProjectionsFromGroupAsync();

            AdjustColumnWidthsSignal = true;

            base.IsBusy = false;
        }



        private async Task RetreiveCashflowProjectionGroupAsync()
        {
            this.cashflowProjectionGroup = await SelectedCashflow.Entity.GenerateProjectionAsync(
                                    SelectedBalanceDate.DateOfBalance.AddDays(1),
                                    Convert.ToInt32(QtyMonths.Value),
                                    OpeningBalance.Value,
                                    Threshold.Value
                                    );
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

            AdjustColumnWidthsSignal = true;
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
