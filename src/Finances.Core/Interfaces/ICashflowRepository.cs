using System.Collections.Generic;
using Finances.Core.Entities;

namespace Finances.Core.Interfaces
{
    public interface ICashflowRepository
    {
        int Add(Cashflow data);
        bool Update(Cashflow data);
        bool Delete(Cashflow data);
        bool Delete(List<int> ids);
        Cashflow Read(int dataId);
        List<Cashflow> ReadList();
        List<DataIdName> ReadListDataIdName();
    }
}
