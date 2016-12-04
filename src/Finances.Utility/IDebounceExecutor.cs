using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Utility
{
    public interface IDebounceExecutor<T>
    {
        void Initialize(TimeSpan ts, Action<T> action);
        void Execute(T item);
    }
}
