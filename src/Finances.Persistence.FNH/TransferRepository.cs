using System.Collections.Generic;
using System.Linq;
using Finances.Core.Interfaces;
using Finances.Core.Entities;
using NHibernate;

namespace Finances.Persistence.FNH
{
    public class TransferRepository : GenericRepository<Transfer>, ITransferRepository
    {
        readonly ISessionFactory sessionFactory;

        public TransferRepository(ISessionFactory sessionFactory) : base(sessionFactory)
        {
            this.sessionFactory = sessionFactory;
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
