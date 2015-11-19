using System.Collections.Generic;
using Finances.Core.Entities;

namespace Finances.Core.Interfaces
{
    public interface IRepositoryRead<TEntity> where TEntity : Entity
    {
        TEntity Read(int dataId);
        List<TEntity> ReadList();
        List<DataIdName> ReadListDataIdName();
    }
}
