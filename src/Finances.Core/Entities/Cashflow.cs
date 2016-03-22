using System;
using System.Linq;
using System.Collections.Generic;
using Finances.Core.Engines.Cashflow;
using Finances.Core.Interfaces;
using Finances.Core.ValueObjects;
using System.Threading.Tasks;

namespace Finances.Core.Entities
{
    public class Cashflow : Entity
    {
        //readonly ICashflowProjectionTransferGenerator projectionTransferGenerator;
        readonly ICashflowProjection cashflowProjection;

        public Cashflow(ICashflowProjection cashflowProjection)
        {
            //this.projectionTransferGenerator = projectionTransferGenerator;
            this.cashflowProjection = cashflowProjection;

            CashflowBankAccounts = new List<CashflowBankAccount>();
        }

        public int CashflowId { get; set; }
        public string Name { get; set; }
        public decimal OpeningBalance { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime RecordCreatedDateTime { get; set; }
        public DateTime RecordUpdatedDateTime { get; set; }

        public List<CashflowBankAccount> CashflowBankAccounts { get; set; }

        public async Task<List<CashflowProjectionItem>> GenerateProjectionAsync(
                DateTime startDate,
                int months,
                decimal openingBalance,
                decimal threshold,
                ICashflowProjectionMode projectionMode
                )
        {
            var endDate = startDate.AddDays(-startDate.Day).AddDays(1).AddMonths(months + 1).AddDays(-1); // last day of month

            return await GenerateProjectionAsync(startDate, endDate, openingBalance, threshold, projectionMode);
        }

        public async Task<List<CashflowProjectionItem>> GenerateProjectionAsync(
                        DateTime startDate,
                        DateTime endDate,
                        decimal openingBalance,
                        decimal threshold,
                        ICashflowProjectionMode projectionMode
                        )
        {
            return await this.cashflowProjection.GenerateProjectionAsync(this.CashflowBankAccounts, startDate, endDate, openingBalance, threshold, projectionMode);
        }

        public async Task<CashflowProjectionGroup> GenerateProjectionAsync(
                        DateTime startDate,
                        int months,
                        decimal openingBalance,
                        decimal threshold
                        )
        {
            var endDate = startDate.AddDays(-startDate.Day).AddDays(1).AddMonths(months + 1).AddDays(-1); // last day of month
            return await this.cashflowProjection.GenerateProjectionAsync(this.CashflowBankAccounts, startDate, endDate, openingBalance, threshold);
        }


    }
}
