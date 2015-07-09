using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.Entities
{
    public enum BalanceStateEnum
    {
        OK,
        BelowThreshold,
        Negative
    }


    public class CashflowProjectionItem
    {
        public string Period { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public string Item { get; set; }
        public decimal? In { get; set; }
        public decimal? Out { get; set; }
        public decimal Balance { get; set; }
        public decimal? PeriodBalance { get; set; }
        public BalanceStateEnum BalanceState { get; set; }
    }

}
