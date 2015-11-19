using System;
using Finances.Core.Entities;

namespace Finances.Core.ValueObjects
{
    public class TransferDirection
    {
        public Transfer Transfer { get; set; }
        public bool IsInbound { get; set; }
        public bool IsOutbound { get; set; }
    }
}
