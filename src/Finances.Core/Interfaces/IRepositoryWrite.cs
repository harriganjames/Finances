using System.Collections.Generic;
using Finances.Core.Entities;

namespace Finances.Core.Interfaces
{
    public interface IRepositoryWrite<TEntity> where TEntity : Entity
    {
        int Add(TEntity data);
        bool Update(TEntity data);
        bool Delete(TEntity data);
        bool Delete(List<int> ids);
    }
}
