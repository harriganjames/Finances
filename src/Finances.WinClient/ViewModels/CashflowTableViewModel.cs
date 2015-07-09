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
using Remotion.Linq.Collections;
using Finances.Core.Engines.Cashflow;
using Finances.Core.Engines;


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
        readonly ICashflowEngineA cashflowEngineA;
        readonly ICashflowEngineFactoryB cashflowEngineFactoryB;
        readonly ICashflowEngineC cashflowEngineC;
        readonly IEnumerable<IAggregatedProjectionItemsGenerator> aggregatedProjectionItemsGenerators;

        List<Cashflow> cashflowsTemp;
        List<CashflowProjectionItem> cashflowProjectionItems;


        public CashflowTableViewModel(ICashflowRepository cashflowRepository,
                                        ICashflowEngineA cashflowEngineA,
                                        ICashflowEngineFactoryB cashflowEngineFactoryB,
                                        ICashflowEngineC cashflowEngineC,
                                        IEnumerable<IAggregatedProjectionItemsGenerator> aggregatedProjectionItemsGenerators            
                                    )
        {
            this.cashflowRepository = cashflowRepository;
            this.cashflowEngineA = cashflowEngineA;
            this.cashflowEngineFactoryB = cashflowEngineFactoryB;
            this.cashflowEngineC = cashflowEngineC;
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

        List<ProjectionModeEnum> modes;
        public List<ProjectionModeEnum> Modes
        {
            get
            {
                if (modes == null)
                {
                    modes = new List<ProjectionModeEnum>(); // { ProjectionModeEnum.Detail, ProjectionModeEnum.MonthlySummary };
                    foreach (var m in this.aggregatedProjectionItemsGenerators)
                    {
                        modes.Add(m.ProjectionMode);
                    }
                }
                return modes;
            }
        }

        ProjectionModeEnum selectedMode;
        public ProjectionModeEnum SelectedMode
        {
            get
            {
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



        public List<CashflowProjectionItem> CashflowProjectionItems
        {
            get { return cashflowProjectionItems; }
            set 
            { 
                cashflowProjectionItems = value;
                NotifyPropertyChanged(() => CashflowProjectionItems);
            }
        }


        #endregion

        //BalanceState property? negative, below threshold, ok



        private void Refresh()
        {
            GenerateProjection();
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
                .StartNew(() => GenerateProjectionTask())
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
            data.ForEach(d => Cashflows.Add(new CashflowItemViewModel(d)));
            if (Cashflows.Count > 0)
            {
                SelectedCashflow = prevId == 0 ? Cashflows[0] : Cashflows.FirstOrDefault(c => c.CashflowId == prevId);
                NotifyPropertyChanged(() => this.SelectedCashflow);
            }
        }

        private void GenerateProjectionTask()
        {
            if (SelectedCashflow != null)
            {
                // Engine A method
                CashflowProjectionItems = cashflowEngineA.GenerateProjection(SelectedCashflow.Entity.CashflowBankAccounts,
                                                    SelectedCashflow.Entity.StartDate,
                                                    SelectedCashflow.Entity.StartDate.AddMonths(Decimal.ToInt32(QtyMonths.Value)),
                                                    OpeningBalance.Value, Threshold.Value, SelectedMode);


                // Engine B method
                ICashflowEngineB engineB = null;

                if (SelectedMode == ProjectionModeEnum.Detail)
                    engineB = cashflowEngineFactoryB.CreateDetail();
                else if (SelectedMode == ProjectionModeEnum.MonthlySummary)
                    engineB = cashflowEngineFactoryB.CreateMonthlySummary();

                CashflowProjectionItems = engineB.GenerateProjection(SelectedCashflow.Entity.CashflowBankAccounts,
                                    SelectedCashflow.Entity.StartDate,
                                    SelectedCashflow.Entity.StartDate.AddMonths(Decimal.ToInt32(QtyMonths.Value)),
                                    OpeningBalance.Value, Threshold.Value);



                // Engine C method

                var apig = aggregatedProjectionItemsGenerators.FirstOrDefault(g => g.ProjectionMode == SelectedMode);

                CashflowProjectionItems = cashflowEngineC.GenerateProjection(SelectedCashflow.Entity.CashflowBankAccounts,
                                    SelectedCashflow.Entity.StartDate,
                                    SelectedCashflow.Entity.StartDate.AddMonths(Decimal.ToInt32(QtyMonths.Value)),
                                    OpeningBalance.Value, Threshold.Value, apig);



            }
            //.GenerateProjection(SelectedCashflow.Entity,Decimal.ToInt32(QtyMonths.Value),150);
            else
                CashflowProjectionItems = null;
        }

    }

}
