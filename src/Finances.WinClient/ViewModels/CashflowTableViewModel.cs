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
        //readonly IAggregatedProjectionItemsGeneratorFactory aggregatedProjectionItemsGeneratorFactory;

        List<Cashflow> cashflowsTemp;
        List<CashflowProjectionItem> cashflowProjectionItems;


        public CashflowTableViewModel(ICashflowRepository cashflowRepository,
                                        ICashflowEngineA cashflowEngineA,
                                        ICashflowEngineFactoryB cashflowEngineFactoryB,
                                        ICashflowEngineC cashflowEngineC,
                                        IEnumerable<IAggregatedProjectionItemsGenerator> aggregatedProjectionItemsGenerators            
                                        //IAggregatedProjectionItemsGeneratorFactory aggregatedProjectionItemsGeneratorFactory
                                        )
        {
            this.cashflowRepository = cashflowRepository;
            this.cashflowEngineA = cashflowEngineA;
            this.cashflowEngineFactoryB = cashflowEngineFactoryB;
            this.cashflowEngineC = cashflowEngineC;
            this.aggregatedProjectionItemsGenerators = aggregatedProjectionItemsGenerators;
            //this.aggregatedProjectionItemsGeneratorFactory = aggregatedProjectionItemsGeneratorFactory;

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

        List<IAggregatedProjectionItemsGenerator> modes;
        public List<IAggregatedProjectionItemsGenerator> Modes
        {
            get
            {
                if (modes == null)
                {
                    modes = new List<IAggregatedProjectionItemsGenerator>(); 
                    foreach (var m in this.aggregatedProjectionItemsGenerators)
                    {
                        modes.Add(m);
                    }
                    SelectedMode = modes.FirstOrDefault();
                }
                return modes;
            }
        }

        IAggregatedProjectionItemsGenerator selectedMode;
        public IAggregatedProjectionItemsGenerator SelectedMode
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

        //List<DataIdName> modes;
        //public List<DataIdName> Modes
        //{
        //    get
        //    {
        //        if (modes == null)
        //        {
        //            modes = new List<DataIdName>();
        //            foreach (var m in this.aggregatedProjectionItemsGenerators)
        //            {
        //                modes.Add(new DataIdName() { Code = m.ProjectionModeCode, Name = m.ProjectionModeName });
        //            }
        //            SelectedMode = modes.FirstOrDefault();
        //        }
        //        return modes;
        //    }
        //}

        //DataIdName selectedMode;
        //public DataIdName SelectedMode
        //{
        //    get
        //    {
        //        return selectedMode;
        //    }
        //    set
        //    {
        //        if (selectedMode != value)
        //        {
        //            selectedMode = value;
        //            NotifyPropertyChanged(() => SelectedMode);
        //        }
        //    }
        //}


        //List<ProjectionModeEnum> modes_old;
        //public List<ProjectionModeEnum> Modes_old
        //{
        //    get
        //    {
        //        if (modes_old == null)
        //        {
        //            modes_old = new List<ProjectionModeEnum>(); // { ProjectionModeEnum.Detail, ProjectionModeEnum.MonthlySummary };
        //            foreach (var m in this.aggregatedProjectionItemsGenerators)
        //            {
        //                modes_old.Add(m.ProjectionMode);
        //            }
        //        }
        //        return modes_old;
        //    }
        //}

        //ProjectionModeEnum selectedMode_old;
        //public ProjectionModeEnum SelectedMode_old
        //{
        //    get
        //    {
        //        return selectedMode_old;
        //    }
        //    set
        //    {
        //        if (selectedMode_old != value)
        //        {
        //            selectedMode_old = value;
        //            NotifyPropertyChanged(() => SelectedMode_old);
        //        }
        //    }
        //}



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
                //CashflowProjectionItems = cashflowEngineA.GenerateProjection(SelectedCashflow.Entity.CashflowBankAccounts,
                //                                    SelectedCashflow.Entity.StartDate,
                //                                    SelectedCashflow.Entity.StartDate.AddMonths(Decimal.ToInt32(QtyMonths.Value)),
                //                                    OpeningBalance.Value, Threshold.Value, SelectedMode_old);


                // Engine B method
                //ICashflowEngineB engineB = null;

                //if (SelectedMode_old == ProjectionModeEnum.Detail)
                //    engineB = cashflowEngineFactoryB.CreateDetail();
                //else if (SelectedMode_old == ProjectionModeEnum.MonthlySummary)
                //    engineB = cashflowEngineFactoryB.CreateMonthlySummary();

                //CashflowProjectionItems = engineB.GenerateProjection(SelectedCashflow.Entity.CashflowBankAccounts,
                //                    SelectedCashflow.Entity.StartDate,
                //                    SelectedCashflow.Entity.StartDate.AddMonths(Decimal.ToInt32(QtyMonths.Value)),
                //                    OpeningBalance.Value, Threshold.Value);



                // Engine C method

                //var apig = aggregatedProjectionItemsGenerators.FirstOrDefault(g => g.ProjectionMode == SelectedMode);

                //var apig = this.aggregatedProjectionItemsGeneratorFactory.Create(SelectedMode);

                CashflowProjectionItems = cashflowEngineC.GenerateProjection(SelectedCashflow.Entity.CashflowBankAccounts,
                                    SelectedCashflow.Entity.StartDate,
                                    SelectedCashflow.Entity.StartDate.AddMonths(Decimal.ToInt32(QtyMonths.Value)),
                                    OpeningBalance.Value, Threshold.Value, SelectedMode);



            }
            //.GenerateProjection(SelectedCashflow.Entity,Decimal.ToInt32(QtyMonths.Value),150);
            else
                CashflowProjectionItems = null;
        }

    }

}
