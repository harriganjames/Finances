using System;
using System.Collections.Generic;
using System.Linq;

using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Core.ValueObjects;
using Finances.Core.Factories;
using System.Threading.Tasks;

namespace Finances.Core.Engines.Cashflow
{

    public class CashflowProjection : ICashflowProjection
    {
        readonly ICashflowProjectionTransferGenerator projectionTransferGenerator;
        readonly ICashflowProjectionGroupFactory cashflowProjectionGroupFactory;

        public CashflowProjection(ICashflowProjectionTransferGenerator projectionTransferGenerator,
                    ICashflowProjectionGroupFactory cashflowProjectionGroupFactory)
        {
            this.projectionTransferGenerator = projectionTransferGenerator;
            this.cashflowProjectionGroupFactory = cashflowProjectionGroupFactory;
        }

        public async Task<CashflowProjectionGroup> GenerateProjectionAsync(
                List<CashflowBankAccount> accounts,
                DateTime startDate,
                DateTime endDate,
                decimal openingBalance,
                decimal threshold
                )
        {
            var cpg = this.cashflowProjectionGroupFactory.Create();

            List<CashflowProjectionTransfer> cpts = await projectionTransferGenerator.GenerateCashflowProjectionTransfersAsync(accounts, startDate, endDate);

            var tasks = new List<Task<Tuple<ICashflowProjectionMode,List<CashflowProjectionItem>>>>();
            foreach (var mode in cpg.Modes)
            {
                tasks.Add(Task.Factory.StartNew(()=> {
                    var cpi = mode.GenerateAggregatedProjectionItems(cpts);
                    ApplyBalancesAndThreshold(cpi, startDate, openingBalance, threshold);
                    return new Tuple<ICashflowProjectionMode, List<CashflowProjectionItem>>(mode, cpi);
                }));
            }


            var cpgs = await Task.WhenAll(tasks);

            foreach (var t in cpgs)
            {
                cpg.Items.Add(t.Item1, t.Item2);
            }

            //foreach (var mode in cpg.Modes)
            //{
            //    var cpi = mode.GenerateAggregatedProjectionItems(cpts);
            //    ApplyBalancesAndThreshold(cpi, startDate, openingBalance, threshold);
            //    cpg.Items.Add(mode, cpi);
            //}

            return cpg;
        }



        public async Task<List<CashflowProjectionItem>> GenerateProjectionAsync(
                List<CashflowBankAccount> accounts,
                DateTime startDate,
                DateTime endDate,
                decimal openingBalance,
                decimal threshold,
                ICashflowProjectionMode projectionMode
                )
        {
            List<CashflowProjectionTransfer> cpts = await projectionTransferGenerator.GenerateCashflowProjectionTransfersAsync(accounts, startDate, endDate);

            List<CashflowProjectionItem> cpis = projectionMode.GenerateAggregatedProjectionItems(cpts);

            ApplyBalancesAndThreshold(cpis, startDate, openingBalance, threshold);

            return cpis;
        }

        

        List<CashflowProjectionItem> ApplyBalancesAndThreshold(List<CashflowProjectionItem> cashflowProjectionItems,
                                                        DateTime startDate,
                                                        decimal openingBalance,
                                                        decimal threshold)
        {

            cashflowProjectionItems.Insert(0, new CashflowProjectionItem()
            {
                Period = startDate.ToString("yyyy-MM-dd"),
                Item = "Opening Balance",
                In = null,
                Out = null,
                Balance = openingBalance
            });

            // calculate the running balances
            CashflowProjectionItem previousCpi = null;
            decimal runningBalance = openingBalance;
            foreach (var cpi in cashflowProjectionItems.Skip(1))
            {
                if (previousCpi != null && previousCpi.Period != cpi.Period)
                {
                    previousCpi.Balance = runningBalance;
                }
                runningBalance += (cpi.In ?? 0M) - (cpi.Out ?? 0M);

                previousCpi = cpi;
            }
            if(previousCpi!=null)
                previousCpi.Balance = runningBalance;


            // set the BalanceState for threshold
            foreach (var cpi in cashflowProjectionItems)
            {
                if (cpi.Balance.HasValue)
                    cpi.BalanceState = GetBalanceState(cpi.Balance, threshold);
            }

            return cashflowProjectionItems;
        }

        BalanceState GetBalanceState(decimal? balance, decimal threshold)
        {
            BalanceState state = null;
            if (balance.HasValue)
            {
                if (balance < 0)
                    state = new BalanceStateNegative();
                else if (balance < threshold)
                    state = new BalanceStateBelowThreshold();
                else
                    state = new BalanceStateOK();
            }
            return state;
        }


    }
}
