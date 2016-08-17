using System;
using Finances.Core.Entities;

namespace Finances.Core.Factories
{
    public interface IBalanceDateFactory
    {
        BalanceDate Create();
        void Release(BalanceDate s);
    }

}
