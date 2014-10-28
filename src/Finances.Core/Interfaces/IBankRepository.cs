using System.Collections.Generic;
using Finances.Core.Entities;

namespace Finances.Core.Interfaces
{
    public interface IBankRepository : IGenericRepository<Bank>
    {
        //int Add(Bank bank);
        //bool Update(Bank bank);
        //bool Delete(Bank bank);
        ////bool Delete(int bankId);
        //Bank Read(int bankId);
        //List<Bank> ReadList();
        List<DataIdName> ReadListDataIdName();
    }
}
