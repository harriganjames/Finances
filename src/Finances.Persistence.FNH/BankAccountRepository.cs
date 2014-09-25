using System.Collections.Generic;
using System.Linq;
using Finances.Core.Interfaces;
using Finances.Core.Entities;
using NHibernate;
using NHibernate.Linq;

namespace Finances.Persistence.FNH
{
    public class BankAccountRepository : IBankAccountRepository
    {
        readonly ISessionFactory sessionFactory;

        public BankAccountRepository(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public int Add(BankAccount account)
        {
            int id;
            using (var session = this.sessionFactory.OpenSession())
            {
                id = (int)session.Save(account);
                session.Flush();
            }
            return id;
        }


        public bool Update(BankAccount account)
        {
            using (var session = this.sessionFactory.OpenSession())
            {
                session.Update(account);
                session.Flush();
            }
            return true;
        }


        public bool Delete(int accountId)
        {
            using (var session = this.sessionFactory.OpenSession())
            {
                //BankAccount a = new BankAccount() { BankAccountId = accountId };
                //session.Delete(a);
                //session.Save(a);
                //session.Flush();

                IQuery q = session.CreateQuery("delete BankAccount where BankAccountId=?");
                q.SetInt32(0, accountId);
                q.ExecuteUpdate();

            }
            return true;
        }


        public BankAccount Read(int accountId)
        {
            using (var session = this.sessionFactory.OpenSession())
            {
                //return session.Get<BankAccount>(accountId);
                //return session.CreateCriteria<BankAccount>().SetFetchMode("Bank",FetchMode.Join).Add(

                //return session.QueryOver<BankAccount>()
                //    .Where(a=>a.BankAccountId==accountId)
                //    .Fetch(a=>a.Bank).Eager.List().First();


                return (from a in session.Query<BankAccount>().Fetch(a => a.Bank)
                        where a.BankAccountId == accountId
                        select a).Single();

                //return (from a in session.Query<BankAccount>()
                //         where a.BankAccountId==accountId).First();
            }
        }

 

        public List<BankAccount> ReadList()
        {
            using (ISession session = this.sessionFactory.OpenSession())
            {
                //return session.CreateCriteria<BankAccount>().List<BankAccount>().AsEnumerable().ToList();
                //return session.CreateCriteria<BankAccount>().SetFetchMode("",FetchMode.Eager).List<BankAccount>().AsEnumerable().ToList();

                //var a =(from ac in session.QueryOver<BankAccount>().Fetch(acc => acc.Bank).Eager
                //        where ac.BankAccountId>=0
                //        select ac);
                
                //var b = a.List();
                //var c = b.AsEnumerable();
                //var d = c.ToList();

                var query = (from ac in session.Query<BankAccount>().Fetch(a => a.Bank)
                             select ac);

                var x = query.ToList();
                
                return x;
            }
        }

        public List<DataIdName> ReadListDataIdName()
        {
            using (ISession session = this.sessionFactory.OpenSession())
            {
                return (from a in session.CreateCriteria<BankAccount>().List<BankAccount>().AsEnumerable()
                        select new DataIdName() { Id = a.BankAccountId, Name = string.Format("{0} - {1}", a.Bank.Name, a.Name) }).ToList();
            }
        }

    }
}
