using System.Collections.Generic;
using Finances.Core.Entities;

namespace Finances.Core.Interfaces
{
    public interface IBankRepository : IGenericRepository<Bank>
    {
        //bool Add(Core.Entities.Bank bank);
        //bool Update(Core.Entities.Bank bank);
        //bool Delete(Core.Entities.Bank bank);
        ////bool Delete(int bankId);
        //Core.Entities.Bank Read(int bankId);
        //List<Core.Entities.Bank> ReadList();
        List<Core.Entities.DataIdName> ReadListDataIdName();
    }
}
