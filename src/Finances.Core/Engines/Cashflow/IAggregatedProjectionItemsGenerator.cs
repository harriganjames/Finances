using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.Entities;

namespace Finances.Core.Engines.Cashflow
{
    public interface IAggregatedProjectionItemsGenerator
    {
        List<CashflowProjectionItem> GenerateAggregatedProjectionItems(List<CashflowProjectionTransfer> cashflowProjectionTransfers);
        ProjectionModeEnum ProjectionMode { get; }
    }
}
