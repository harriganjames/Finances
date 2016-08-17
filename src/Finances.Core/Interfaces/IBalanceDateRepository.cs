using System.Collections.Generic;
using Finances.Core.Entities;

namespace Finances.Core.Interfaces
{
    public interface IBalanceDateRepository : IRepository
    {
        int Add(BalanceDate data);
        bool Update(BalanceDate data);
        bool Delete(BalanceDate data);
        bool Delete(List<int> ids);
        BalanceDate Read(int dataId);
        List<BalanceDate> ReadList();
        List<DataIdName> ReadListDataIdName();
    }
}
