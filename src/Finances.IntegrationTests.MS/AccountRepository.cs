using System;
using Finances.Core.Interfaces;
using Finances.Persistence.EF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;

namespace Finances.IntegrationTests.MS
{
    [TestClass]
    public class AccountRepository
    {
        IBankAccountRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            string connectionString = "data source=MIKE_LAPTOP;initial catalog=FinanceINT;integrated security=True;App=MSTest;";
            var mcfactory = new ModelContextFactory(connectionString);

            new Finances.Persistence.EF.MappingCreator().CreateMappings();

            _repository = new BankAccountRepository(mcfactory, Mapper.Engine);

        }


        [TestMethod]
        public void TestBankAccountReadList()
        {
            var list = _repository.ReadList();


            Assert.IsNotNull(list, "list is null");

            if (list.Count > 0)
            {
                Assert.IsNotNull(list[0].Bank,"Bank is null");
            }

        }
    }
}
