using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.ValueObjects
{
    public class BalanceStateOK : BalanceState
    {
        public override string Name
        {
            get
            {
                return "OK";
            }
        }

        public override byte[] ColourRBG
        {
            get
            {
                return new byte[3] { 0, 200, 0 };
            }
        }


    }
}
