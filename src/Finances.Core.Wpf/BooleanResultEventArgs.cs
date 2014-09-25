using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.Wpf
{
    public class BooleanResultEventArgs
    {
        public BooleanResultEventArgs(bool result)
        {
            Result = result;
        }
        public bool Result { get; set; }
    }
}
