using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.ValueObjects
{
    public class BalanceStateNegative : BalanceState
    {
        public override string Name
        {
            get
            {
                return "Negative";
            }
        }

        public override byte[] ColourRBG
        {
            get
            {
                return new byte[3] { 255, 0, 0 };
            }
        }


    }
}
