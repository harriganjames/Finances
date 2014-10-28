using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class, new()
    {
        int Add(T data);
        bool Update(T data);
        bool Delete(T data);
        T Read(int id);
        List<T> ReadList();
    }

}
