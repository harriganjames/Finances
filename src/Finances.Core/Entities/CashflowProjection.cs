using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.Entities
{

    public class CashflowProjection
    {
        List<CashflowProjectionItem> items;

        public List<CashflowProjectionItem> Items
        {
            get 
            {
                if (items == null)
                    items = new List<CashflowProjectionItem>();
                return items; 
            }
        }

        public Cashflow Cashflow { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}
