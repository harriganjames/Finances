using System;
using System.Linq;
using Finances.Core.Interfaces;
using Finances.Persistence.EF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using Finances.Core.Factories;

namespace Finances.IntegrationTests.MS
{
    [TestClass]
    public class BankRepositoryEFTests : IntegrationBase
    {
        IBankRepository repository;
        ITransferFactory transferFactory;

        [TestInitialize]
        public void Initialize()
        {
            //string connectionString = "data source=MIKE_LAPTOP;initial catalog=FinanceINT;integrated security=True;App=MSTest;";
            //var mcfactory = new ModelContextFactory(connectionString);

            //IScheduleFactory scheduleFactory = new Fakes.FakeScheduleFactory();

            //transferFactory = new Fakes.FakeTransferFactory(scheduleFactory);

            //new Finances.Persistence.EF.MappingCreator(transferFactory).CreateMappings();

            //repository = new BankRepository(mcfactory,Mapper.Engine);

            repository = container.Resolve<IBankRepository>();

        }


        [TestMethod]
        public void TestBankReadList()
        {
            var list = repository.ReadList();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public void TestCRUD()
        {
            Core.Entities.Bank read;
            var b = new Core.Entities.Bank() { Name = "TestCRUD-" + Guid.NewGuid().ToString() };

            repository.Add(b);
            Assert.IsTrue(b.BankId > 0,"BankId not set");

            read = repository.Read(b.BankId);
            Assert.IsNotNull(read);
            Assert.AreEqual(read.BankId, b.BankId);

            b.Name += "-UPDATE";
            repository.Update(b);

            read = repository.Read(b.BankId);
            Assert.IsNotNull(read);
            Assert.AreEqual(read.Name, b.Name);

            repository.Delete(b);

            read = repository.Read(b.BankId);
            Assert.IsNull(read);
        }

        [TestMethod]
        public void TestReadListDataIdName()
        {
            var list = repository.ReadListDataIdName();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);

            Assert.IsTrue(list.Count(d => d.Id > 0) == list.Count);
            Assert.IsTrue(list.Count(d => !String.IsNullOrEmpty(d.Name)) == list.Count);
        }

    }
}
