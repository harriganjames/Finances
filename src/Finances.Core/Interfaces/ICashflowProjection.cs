using System;
using System.Collections.Generic;

using Finances.Core.Entities;
using Finances.Core.ValueObjects;

namespace Finances.Core.Interfaces
{
    public interface ICashflowProjection
    {
        List<CashflowProjectionItem> GenerateProjection(
                List<CashflowBankAccount> accounts,
                DateTime startDate,
                DateTime endDate,
                decimal openingBalance,
                decimal threshold,
                ICashflowProjectionMode projectionMode
                );
    }
}
