using System.Collections.Generic;
using System.Linq;
using Finances.Core.Interfaces;
using Finances.Core.Entities;
using NHibernate;
using System;

namespace Finances.Persistence.FNH
{

    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        readonly ISessionFactory sessionFactory;

        public GenericRepository(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public virtual int Add(T data)
        {
            int id;
            using (var session = this.sessionFactory.OpenSession())
            {
                id = (int)session.Save(data);
                session.Flush();
            }
            return id;
        }


        public virtual bool Update(T data)
        {
            using (var session = this.sessionFactory.OpenSession())
            {
                session.Update(data);
                session.Flush();
            }
            return true;
        }


        public virtual bool Delete(T data)
        {
            using (var session = this.sessionFactory.OpenSession())
            {
                session.Delete(data);
                session.Flush();
            }
            return true;
        }


        public virtual T Read(int id)
        {
            using (var session = this.sessionFactory.OpenSession())
            {
                return session.Get<T>(id);
            }
        }


        public virtual List<T> ReadList()
        {
            using (ISession session = this.sessionFactory.OpenSession())
            {
                return session.CreateCriteria<T>().List<T>().AsEnumerable().ToList();
            }
        }


        protected List<DataIdName> ReadListDataIdName(Func<T,int> idFunc, Func<T,string> nameFunc)
        {
            using (ISession session = this.sessionFactory.OpenSession())
            {
                return (from b in session.CreateCriteria<T>().List<T>().AsEnumerable()
                        select new DataIdName() { Id = idFunc(b), Name = nameFunc(b) }).ToList();
            }
        }


    }
}
