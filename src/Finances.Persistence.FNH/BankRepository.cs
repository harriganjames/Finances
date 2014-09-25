using System.Collections.Generic;
using System.Linq;
using Finances.Core.Interfaces;
using Finances.Core.Entities;
using NHibernate;

namespace Finances.Persistence.FNH
{
    public class BankRepository : IBankRepository
    {
        readonly ISessionFactory sessionFactory;

        public BankRepository(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public int Add(Bank bank)
        {
            int id;
            using (var session = this.sessionFactory.OpenSession())
            {
                id = (int)session.Save(bank);
                session.Flush();
            }
            return id;
        }


        public bool Update(Bank bank)
        {
            using (var session = this.sessionFactory.OpenSession())
            {
                session.Update(bank);
                session.Flush();
            }
            return true;
        }


        public bool Delete(int bankId)
        {
            using (var session = this.sessionFactory.OpenSession())
            {
                //session.Delete(bank);
                session.Delete(new Bank() { BankId = bankId });
                session.Flush();
            }
            return true;
        }


        public Bank Read(int bankId)
        {
            using (var session = this.sessionFactory.OpenSession())
            {
                return session.Get<Bank>(bankId);
            }
        }


        public List<Bank> ReadList()
        {
            using (ISession session = this.sessionFactory.OpenSession())
            {
                return session.CreateCriteria<Bank>().List<Bank>().AsEnumerable().ToList();
            }
        }

        public List<DataIdName> ReadListDataIdName()
        {
            using (ISession session = this.sessionFactory.OpenSession())
            {
                return (from b in session.CreateCriteria<Bank>().List<Bank>().AsEnumerable()
                        select new DataIdName() { Id = b.BankId, Name = b.Name }).ToList();
            }
        }

    }
}
