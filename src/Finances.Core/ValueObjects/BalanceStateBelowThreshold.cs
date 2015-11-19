using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.ValueObjects
{
    public class BalanceStateBelowThreshold : BalanceState
    {
        public override string Name
        {
            get
            {
                return "BelowThreshold";
            }
        }

        public override byte[] ColourRBG
        {
            get
            {
                return new byte[3] { 255, 150, 0 };
            }
        }


    }
}
