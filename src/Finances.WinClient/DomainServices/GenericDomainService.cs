using System;
using Finances.Core.Interfaces;
using Finances.Persistence.FNH;

namespace Finances.WinClient.DomainServices
{
    public interface IGenericDomainService<T> where T : class, new()
    {
        bool Add(IEntityMapper<T> vm);
        bool Update(IEntityMapper<T> vm);
        void Read(int id, IEntityMapper<T> vm);
        bool Delete(IEntityMapper<T> vm);
        //List<T> ReadList();
    }


    public class GenericDomainService<T> : IGenericDomainService<T> where T : class, new()
    {
        readonly IGenericRepository<T> repo;

        public GenericDomainService(IGenericRepository<T> repo)
        {
            this.repo = repo;
        }

        public bool Add(IEntityMapper<T> vm)
        {
            T entity = new T();
            vm.MapOut(entity);
            int id = this.repo.Add(entity);
            vm.MapIn(entity);
            return id > 0;
        }

        public bool Update(IEntityMapper<T> vm)
        {
            T entity = new T();
            vm.MapOut(entity);
            return this.repo.Update(entity);
        }

        public void Read(int id, IEntityMapper<T> vm) 
        {
            T entity = this.repo.Read(id);
            if (entity != null)
                vm.MapIn(entity);
        }

        public bool Delete(IEntityMapper<T> vm)
        {
            T entity = new T();
            vm.MapOut(entity);
            return this.repo.Delete(entity);
        }

    }
}
