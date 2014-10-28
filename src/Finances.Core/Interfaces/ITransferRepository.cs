using System.Collections.Generic;
using Finances.Core.Entities;

namespace Finances.Core.Interfaces
{
    public interface ITransferRepository : IGenericRepository<Transfer>
    {
        //int Add(Transfer data);
        //bool Update(Transfer data);
        //bool Delete(Transfer data);
        ////bool Delete(int dataId);
        //Transfer Read(int dataId);
        //List<Transfer> ReadList();
        List<DataIdName> ReadListDataIdName();
    }
}
