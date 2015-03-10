using System.Collections.Generic;
using Finances.Core.Entities;

namespace Finances.Core.Interfaces
{
    public interface ITransferRepository
    {
        int Add(Transfer data);
        bool Update(Transfer data);
        bool Delete(Transfer data);
        Transfer Read(int dataId);
        List<Transfer> ReadList();
        List<DataIdName> ReadListDataIdName();
    }
}
