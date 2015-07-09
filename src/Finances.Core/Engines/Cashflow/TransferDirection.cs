using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.Entities;

namespace Finances.Core.Engines.Cashflow
{
    public class TransferDirection
    {
        public Transfer Transfer { get; set; }
        public bool IsInbound { get; set; }
        public bool IsOutbound { get; set; }
    }
}
