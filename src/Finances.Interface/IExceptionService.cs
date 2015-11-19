using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Interface
{
    public interface IExceptionService
    {
        void ShowException(Exception e);
        void ShowException(Exception e, string title);
    }
}
