using System.Collections.Generic;
using System.Linq;
using Finances.Core.Interfaces;
using Finances.Core.Entities;
using NHibernate;
using NHibernate.Linq;

namespace Finances.Persistence.FNH
{
    public class TransferRepository : GenericRepository<Transfer>, ITransferRepository
    {
        readonly ISessionFactory sessionFactory;

        public TransferRepository(ISessionFactory sessionFactory) : base(sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }


        // overriding so can Fetch 
        public override Transfer Read(int transferId)
        {
            using (ISession session = this.sessionFactory.OpenSession())
            {
                return (from t in session.Query<Transfer>()
                                 .Fetch(t => t.FromBankAccount).ThenFetch(a => a.Bank)
                                 .Fetch(t => t.ToBankAccount).ThenFetch(a => a.Bank)
                        where t.TransferId == transferId
                        select t).Single();
            }
        }


        // overriding so can Fetch 
        public override List<Transfer> ReadList() 
        {
            using (ISession session = this.sessionFactory.OpenSession())
            {
                var query = (from ac in session.Query<Transfer>()
                                 .Fetch(t => t.FromBankAccount).ThenFetch(a => a.Bank)
                                 .Fetch(t => t.ToBankAccount).ThenFetch(a => a.Bank)
                             select ac);

                var x = query.ToList();

                return x;
            }
        }


        public List<DataIdName> ReadListDataIdName()
        {
            return base.ReadListDataIdName(t => t.TransferId, t => t.Name);
        }

        //private List<DataIdName> ReadListDataIdName_old()
        //{
        //    using (ISession session = this.sessionFactory.OpenSession())
        //    {
        //        return (from b in session.CreateCriteria<Bank>().List<Bank>().AsEnumerable()
        //                select new DataIdName() { Id = b.BankId, Name = b.Name }).ToList();
        //    }
        //}

    }
}
