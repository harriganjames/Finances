using System;
using Finances.Core.Interfaces;
using Finances.Persistence.EF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;

namespace Finances.IntegrationTests.MS
{
    [TestClass]
    public class BankRepositoryEFTests
    {
        IBankRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            string connectionString = "data source=MIKE_LAPTOP;initial catalog=FinanceINT;integrated security=True;App=MSTest;";
            var mcfactory = new ModelContextFactory(connectionString);

            new Finances.Persistence.EF.MappingCreator().CreateMappings();

            _repository = new BankRepository(mcfactory,Mapper.Engine);

        }


        [TestMethod]
        public void TestBankReadList()
        {
            var list = _repository.ReadList();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public void TestCRUD()
        {
            Core.Entities.Bank read;
            var b = new Core.Entities.Bank() { Name = "TestCRUD-" + Guid.NewGuid().ToString() };

            _repository.Add(b);
            Assert.IsTrue(b.BankId > 0,"BankId not set");

            read = _repository.Read(b.BankId);
            Assert.IsNotNull(read);
            Assert.AreEqual(read.BankId, b.BankId);

            b.Name += "-UPDATE";
            _repository.Update(b);

            read = _repository.Read(b.BankId);
            Assert.IsNotNull(read);
            Assert.AreEqual(read.Name, b.Name);

            _repository.Delete(b);

            read = _repository.Read(b.BankId);
            Assert.IsNull(read);
        }

    }
}
