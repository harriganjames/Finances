using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.ValueObjects;

namespace Finances.Core.ValueObjects
{
    public class CashflowProjectionTransfer
    {
        public DateTime Date { get; set; }
        public TransferDirection TransferDirection { get; set; }
    }
}
