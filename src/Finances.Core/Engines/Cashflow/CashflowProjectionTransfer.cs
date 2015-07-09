using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.Engines.Cashflow
{
    public class CashflowProjectionTransfer
    {
        public DateTime Date { get; set; }
        public TransferDirection TransferDirection { get; set; }
    }
}
