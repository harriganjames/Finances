using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.Exceptions
{
    public class FinancesCoreException : Exception
    {
        public FinancesCoreException(string message, params object[] args)
            : base(String.Format(message, args))  
        {
        }
    }
}
