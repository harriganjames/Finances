using System;
using System.Collections.Generic;
using Finances.Core.Interfaces;
using Finances.Persistence.FNH;

namespace Finances.WinClient.DomainServices
{
    public interface IGenericDomainService<TEntity> where TEntity : class, new()
    {
        bool Add(IEntityMapper<TEntity> vm);
        bool Update(IEntityMapper<TEntity> vm);
        void Read(int id, IEntityMapper<TEntity> vm);
        bool Delete(IEntityMapper<TEntity> vm);
        //List<TEntity> ReadList();
    }


    public class GenericDomainService<TEntity> : IGenericDomainService<TEntity> where TEntity : class, new()
    {
        readonly IGenericRepository<TEntity> repo;

        public GenericDomainService(IGenericRepository<TEntity> repo)
        {
            this.repo = repo;
        }

        public bool Add(IEntityMapper<TEntity> vm)
        {
            TEntity entity = new TEntity();
            vm.MapOut(entity);
            int id = this.repo.Add(entity);
            vm.MapIn(entity);
            return id > 0;
        }

        public bool Update(IEntityMapper<TEntity> vm)
        {
            TEntity entity = new TEntity();
            vm.MapOut(entity);
            return this.repo.Update(entity);
        }

        public void Read(int id, IEntityMapper<TEntity> vm) 
        {
            TEntity entity = this.repo.Read(id);
            if (entity != null)
                 vm.MapIn(entity);
        }

        public bool Delete(IEntityMapper<TEntity> vm)
        {
            TEntity entity = new TEntity();
            vm.MapOut(entity);
            return this.repo.Delete(entity);
        }


        public List<IMapper> ReadList<TMapper, IMapper>() 
                                    where TMapper : IMapper, new()
                                    where IMapper : IEntityMapper<TEntity>
        {
            List<IMapper> mappers = new List<IMapper>();
            List<TEntity> entities = this.repo.ReadList();
            if (entities != null)
            {
                foreach (TEntity e in entities)
                {
                    IMapper m = new TMapper();
                    m.MapIn(e);
                    mappers.Add(m);
                }
            }
            return mappers;
        }

        public List<IMapper> ReadListFunc<TMapper, IMapper>(Func<List<TEntity>> func)
            where TMapper : IMapper, new()
            where IMapper : IEntityMapper<TEntity>
        {
            List<IMapper> mappers = new List<IMapper>();
            List<TEntity> entities = func();
            if (entities != null)
            {
                foreach (TEntity e in entities)
                {
                    IMapper m = new TMapper();
                    m.MapIn(e);
                    mappers.Add(m);
                }
            }
            return mappers;
        }


    }
}
