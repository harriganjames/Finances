using System.Collections.Generic;
using System.Linq;
using Finances.Core.Interfaces;
using Finances.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System;

namespace Finances.Persistence.FNH
{
    public class BankAccountRepository : GenericRepository<BankAccount>, IBankAccountRepository
    {
        readonly ISessionFactory sessionFactory;

        public BankAccountRepository(ISessionFactory sessionFactory) : base(sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }


        // overriding so can use SQL to avoid cascaded deletes
        public override bool Delete(BankAccount bankAccount)
        {
            using (var session = this.sessionFactory.OpenSession())
            {
                IQuery q = session.CreateQuery("delete BankAccount where BankAccountId=?");
                q.SetInt32(0, bankAccount.BankAccountId);
                q.ExecuteUpdate();

            }
            return true;
        }


        // Extra method - uses SQL to avoid cascaded deletes
        //public bool Delete(int accountId)
        //{
        //    using (var session = this.sessionFactory.OpenSession())
        //    {
        //        IQuery q = session.CreateQuery("delete BankAccount where BankAccountId=?");
        //        q.SetInt32(0, accountId);
        //        q.ExecuteUpdate();
        //    }
        //    return true;
        //}


        // overriding so can Fetch Bank
        public override BankAccount Read(int accountId)
        {
            using (var session = this.sessionFactory.OpenSession())
            {
                return (from a in session.Query<BankAccount>().Fetch(a => a.Bank)
                        where a.BankAccountId == accountId
                        select a).Single();
            }
        }

 
        // overriding so can Fetch Bank
        public override List<BankAccount> ReadList()
        {
            using (ISession session = this.sessionFactory.OpenSession())
            {
                var query = (from ac in session.Query<BankAccount>().Fetch(a => a.Bank)
                             select ac);

                var x = query.ToList();
                
                return x;
            }
        }

        public List<BankAccount> ReadListByBankId(int bankId)
        {
            using (ISession session = this.sessionFactory.OpenSession())
            {
                var query = (from ac in session.Query<BankAccount>().Fetch(a => a.Bank)
                             where ac.Bank.BankId==bankId
                             select ac);

                var x = query.ToList();

                return x;
            }
        }


        public List<DataIdName> ReadListDataIdName()
        {
            return base.ReadListDataIdName(a => a.BankAccountId, a => String.Format("{0} - {1}", a.Bank.Name, a.Name));
        }

        //public List<DataIdName> ReadListDataIdName()
        //{
        //    using (ISession session = this.sessionFactory.OpenSession())
        //    {
        //        return (from a in session.CreateCriteria<BankAccount>().List<BankAccount>().AsEnumerable()
        //                select new DataIdName() { Id = a.BankAccountId, Name = string.Format("{0} - {1}", a.Bank.Name, a.Name) }).ToList();
        //    }
        //}

    }
}
