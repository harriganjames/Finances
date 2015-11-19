using System;
using System.Collections.Generic;
using System.Linq;

using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Core.ValueObjects;

namespace Finances.Core.Engines.Cashflow
{

    public class CashflowProjection : ICashflowProjection
    {
        readonly ICashflowProjectionTransferGenerator projectionTransferGenerator;

        public CashflowProjection(ICashflowProjectionTransferGenerator projectionTransferGenerator)
        {
            this.projectionTransferGenerator = projectionTransferGenerator;

        }

        public List<CashflowProjectionItem> GenerateProjection(
                List<CashflowBankAccount> accounts,
                DateTime startDate,
                DateTime endDate,
                decimal openingBalance,
                decimal threshold,
                ICashflowProjectionMode projectionMode
                )
        {
            List<CashflowProjectionTransfer> cpts = projectionTransferGenerator.GenerateCashflowProjectionTransfers(accounts, startDate, endDate);

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


            var previousCpi = cashflowProjectionItems[0]; // start with opening balance item
            foreach (var cpi in cashflowProjectionItems.Skip(1))
            {
                if (previousCpi.Period != cpi.Period)
                    previousCpi.PeriodBalance = previousCpi.Balance;
                cpi.Balance = previousCpi.Balance + (cpi.In ?? 0M) - (cpi.Out ?? 0M);
                previousCpi = cpi;
            }
            previousCpi.PeriodBalance = previousCpi.Balance;

            // set the BalanceState for threshold
            foreach (var cpi in cashflowProjectionItems)
            {
                if (cpi.Balance < 0)
                    cpi.BalanceState = new BalanceStateNegative();
                else if (cpi.Balance < threshold)
                    cpi.BalanceState = new BalanceStateBelowThreshold();
                else
                    cpi.BalanceState = new BalanceStateOK();
            }

            return cashflowProjectionItems;
        }

    }
}
