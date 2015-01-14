using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentNHibernate.Cfg;
using NHibernate;

using Finances.Persistence.FNH;
using Finances.Persistence.FNH.Mappings;

namespace Finances.UnitTests.MS
{
    [TestClass]
    public class GeneralTests
    {
        GeneralRepository _generalRepository;

        [TestInitialize]
        public void Initialize()
        {
            string conn = "Server=localhost; Database=Finance; Integrated Security=true";// ConfigurationManager.AppSettings["localDB"];

            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2008.ConnectionString(c => c.Is(conn))
                    .ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<BankMap>()
                    .ExportTo(@"C:\Temp"))
                .BuildSessionFactory();

            _generalRepository = new GeneralRepository(sessionFactory);
        }


        [TestMethod]
        public void TestGetDataProc()
        {
            var data = _generalRepository.GetDataTestProc();
        }

        [TestMethod]
        public void TestGetDataQuery()
        {
            var data = _generalRepository.GetDataTestQuery();
        }

        [TestMethod]
        public void TestGetBankById()
        {
            var bank1 = _generalRepository.GetBankById(0);
            var bank2 = _generalRepository.GetBankById(5);
        }

        [TestMethod]
        public void TestInsertBankAndReadId()
        {
            var id = _generalRepository.InsertBankAndReadId(String.Format("Test {0}", DateTime.Now.TimeOfDay.ToString()));
        }

    }
}
