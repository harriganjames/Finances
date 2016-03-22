using System;
using System.Collections.Generic;
using System.Linq;

using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Core.ValueObjects;

namespace Finances.Core.Engines.Cashflow
{
    public class CashflowProjectionModeDetail : ICashflowProjectionMode
    {

        public string ProjectionModeName
        {
            get
            {
                return "Detail";
            }
        }

        public int Order
        {
            get
            {
                return 2;
            }
        }

        public override string ToString()
        {
            return this.ProjectionModeName;
        }

        public List<CashflowProjectionItem> GenerateAggregatedProjectionItems(List<CashflowProjectionTransfer> cashflowProjectionTransfers)
        {
            var cpis = new List<CashflowProjectionItem>();

            foreach (var cpt in cashflowProjectionTransfers.OrderBy(c => c.Date))
            {
                var cpi = new CashflowProjectionItem()
                {
                    PeriodGroup = cpt.Date.ToString("yyyy-MM"),
                    Period = cpt.Date.ToString("yyyy-MM-dd"),
                    PeriodStartDate = cpt.Date,
                    PeriodEndDate = cpt.Date,
                    Item = (cpt.TransferDirection.Transfer.Category.Code == "NONE" ? "" : cpt.TransferDirection.Transfer.Category.Name + " -> ") + cpt.TransferDirection.Transfer.Name,
                    In = cpt.TransferDirection.IsInbound ? cpt.TransferDirection.Transfer.Amount : (decimal?)null,
                    Out = cpt.TransferDirection.IsOutbound ? cpt.TransferDirection.Transfer.Amount : (decimal?)null,
                    CashflowProjectionMode = this
                };

                cpis.Add(cpi);
            }

            return cpis;
        }
    }
}
