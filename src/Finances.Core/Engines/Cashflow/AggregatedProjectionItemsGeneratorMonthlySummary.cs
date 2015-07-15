using System;
using System.Collections.Generic;
using System.Linq;

using Finances.Core.Entities;
using Finances.Core.Interfaces;

namespace Finances.Core.Engines.Cashflow
{
    public class AggregatedProjectionItemsGeneratorMonthlySummary : IAggregatedProjectionItemsGenerator
    {
        public ProjectionModeEnum ProjectionMode
        {
            get
            {
                return ProjectionModeEnum.MonthlySummary;
            }
        }

        public List<Entities.CashflowProjectionItem> GenerateAggregatedProjectionItems(List<CashflowProjectionTransfer> cashflowProjectionTransfers)
        {
            var cpis = new List<CashflowProjectionItem>();

            var x = from cpt in cashflowProjectionTransfers.OrderBy(c => c.Date)
                    group cpt by new
                    {
                        Year = cpt.Date.Year,
                        Month = cpt.Date.Month,
                        Item = (cpt.TransferDirection.Transfer.Category.Code == "NONE" ? cpt.TransferDirection.Transfer.Name : cpt.TransferDirection.Transfer.Category.Name),
                    }
                        into grp
                        let inAmount = grp.Sum(c => c.TransferDirection.IsInbound ? c.TransferDirection.Transfer.Amount : 0M)
                        let outAmount = grp.Sum(c => c.TransferDirection.IsOutbound ? c.TransferDirection.Transfer.Amount : 0M)
                        select new CashflowProjectionItem
                        {
                            Period = new DateTime(grp.Key.Year, grp.Key.Month, 1).ToString("yyyy-MM"),
                            PeriodStartDate = grp.Min(c => c.Date),
                            PeriodEndDate = grp.Max(c => c.Date),
                            Item = grp.Key.Item,
                            In = inAmount == 0 ? (decimal?)null : inAmount,
                            Out = outAmount == 0 ? (decimal?)null : outAmount
                        };

            cpis.AddRange(x);

            return cpis;
        }

    }
}
