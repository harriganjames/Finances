using System;
using System.Collections.Generic;
using Finances.Core.Engines.Cashflow;
using Finances.Core.Entities;

namespace Finances.Core.Interfaces
{
    public interface ICashflowEngineB
    {
        List<CashflowProjectionItem> GenerateProjection(
                                List<CashflowBankAccount> accounts,
                                DateTime startDate,
                                DateTime endDate,
                                decimal openingBalance,
                                decimal threshold);
    }
}
