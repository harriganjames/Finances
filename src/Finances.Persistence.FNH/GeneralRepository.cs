using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using NHibernate;
//using NHibernate.Linq;

namespace Finances.Persistence.FNH
{
    public class GeneralRepository : IGeneralRepository
    {
        readonly ISessionFactory sessionFactory;
        public GeneralRepository(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }


        public List<Bank> GetDataTestProc()
        {
            List<Bank> result = null;
            using (var session = this.sessionFactory.OpenSession())
            {
                result = session.CreateSQLQuery("exec GetDataTest")
                    .AddEntity(typeof(Bank))
                    .List<Bank>()
                    .ToList();
            }
            return result;
        }

        public List<Bank> GetDataTestQuery()
        {
            List<Bank> result = null;
            using (var session = this.sessionFactory.OpenSession())
            {
                result = session.CreateSQLQuery("select * from dbo.Bank")
                    .AddEntity(typeof(Bank))
                    .List<Bank>()
                    .ToList();
            }
            return result;
        }

        public Bank GetBankById(int bankId)
        {
            Bank result = null;
            using (var session = this.sessionFactory.OpenSession())
            {
                result = session.CreateSQLQuery("select BankId, Name, Logo from dbo.Bank where BankId=:BankId")
                    .AddEntity(typeof(Bank))
                    .SetInt32("BankId", bankId)
                    .UniqueResult<Bank>();
            }
            return result;
        }

        public int InsertBankAndReadId(string name)
        {
            int result;
            using (var session = this.sessionFactory.OpenSession())
            {
                result = session.CreateSQLQuery("insert dbo.Bank (Name) values (:Name) select scope_identity() as id")
                    .AddScalar("id", new NHibernate.Type.Int32Type())
                    .SetString("Name", name)
                    .UniqueResult<int>()
                    ;
            }
            return result;
        }


    }
}
