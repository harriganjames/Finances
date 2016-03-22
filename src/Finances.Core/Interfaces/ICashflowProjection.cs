using System;
using System.Collections.Generic;

using Finances.Core.Entities;
using Finances.Core.ValueObjects;
using Finances.Core.Engines.Cashflow;
using System.Threading.Tasks;

namespace Finances.Core.Interfaces
{
    public interface ICashflowProjection
    {
        Task<List<CashflowProjectionItem>> GenerateProjectionAsync(
                List<CashflowBankAccount> accounts,
                DateTime startDate,
                DateTime endDate,
                decimal openingBalance,
                decimal threshold,
                ICashflowProjectionMode projectionMode
                );
        Task<CashflowProjectionGroup> GenerateProjectionAsync(
                List<CashflowBankAccount> accounts,
                DateTime startDate,
                DateTime endDate,
                decimal openingBalance,
                decimal threshold
                );

    }
}
