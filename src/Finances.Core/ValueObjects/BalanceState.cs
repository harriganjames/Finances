using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.ValueObjects
{
    public abstract class BalanceState
    {
        public abstract string Name{ get; }
        public abstract byte[] ColourRBG { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
