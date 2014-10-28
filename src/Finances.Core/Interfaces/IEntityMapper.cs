using Finances.Core.Entities;

namespace Finances.Core.Interfaces
{
    public interface IEntityMapper<T>
    {
        void MapIn(T entity);
        void MapOut(T entity);
    }
}
