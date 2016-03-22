using System;
using Finances.Core.Entities;
using Finances.Core.Engines.Cashflow;

namespace Finances.Core.Factories
{
    public interface ICashflowProjectionGroupFactory
    {
        CashflowProjectionGroup Create();
        void Release(CashflowProjectionGroup s);
    }

}
