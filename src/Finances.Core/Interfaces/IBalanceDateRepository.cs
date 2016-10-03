using System.Collections.Generic;
using Finances.Core.Entities;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Finances.Core.Interfaces
{
    public interface IBalanceDateRepository : IRepository
    {
        int Add(BalanceDate data);
        bool Update(BalanceDate data);
        bool Delete(BalanceDate data);
        bool Delete(List<int> ids);
        BalanceDate Read(int dataId);
        Task PostList(ITargetBlock<Core.Entities.BalanceDate> target);
        List<BalanceDate> ReadList();
        Task<List<Core.Entities.BalanceDate>> ReadListAsync();
        List<DataIdName> ReadListDataIdName();
    }
}
