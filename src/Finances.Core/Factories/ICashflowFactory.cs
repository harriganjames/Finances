using System;
using Finances.Core.Entities;

namespace Finances.Core.Factories
{
    public interface ICashflowFactory
    {
        Cashflow Create();
        void Release(Cashflow s);
    }

}
