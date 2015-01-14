using System;
using Finances.Core.Interfaces;
using Finances.Persistence.EF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using System.Linq;

namespace Finances.IntegrationTests.MS
{
    [TestClass]
    public class BankAccountRepositoryEFTests
    {
        IBankAccountRepository _repository;
        Core.Entities.BankAccount testEntity;
        //Core.Entities.Bank testBank;

        [TestInitialize]
        public void Initialize()
        {
            string connectionString = "data source=MIKE_LAPTOP;initial catalog=FinanceINT;integrated security=True;App=MSTest;";
            var mcfactory = new ModelContextFactory(connectionString);

            new Finances.Persistence.EF.MappingCreator().CreateMappings();

            _repository = new BankAccountRepository(mcfactory,Mapper.Engine);


            testEntity = new Core.Entities.BankAccount()
            {
                Bank = new Core.Entities.Bank() { BankId = 1 },
                AccountNumber = "1234",
                AccountOwner = "me",
                Name = "Test-" + Guid.NewGuid().ToString(),
                LoginID = "login",
                LoginURL = "abc",
                PasswordHint = "hint",
                OpenedDate = DateTime.Now.Date,
                ClosedDate = DateTime.Now.Date.AddDays(1),
                SortCode = "000010",
                InitialRate = 1.1M,
                PaysTaxableInterest = true,
                MilestoneDate = DateTime.Now.Date.AddDays(2),
                MilestoneNotes = "msnotes",
                Notes = "test notes"
            };



        }




        [TestMethod]
        public void TestCRUD()
        {
            Core.Entities.BankAccount read;
            var entity = testEntity;// new Core.Entities.BankAccount() { Name = "TestCRUD-" + Guid.NewGuid().ToString() };

            _repository.Add(entity);
            Assert.IsTrue(entity.BankAccountId > 0, "BankAccountId not set");

            read = _repository.Read(entity.BankAccountId);
            Assert.IsNotNull(read);
            CompareBankAccounts(entity, read, "Read");

            entity.Name += "-UPDATE";
            _repository.Update(entity);
            read = _repository.Read(entity.BankAccountId);
            Assert.IsNotNull(read);
            CompareBankAccounts(entity, read, "Update");

            _repository.Delete(entity);

            read = _repository.Read(entity.BankAccountId);
            Assert.IsNull(read);
        }


        [TestMethod]
        public void TestReadList()
        {
            var list = _repository.ReadList();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public void TestReadListByBankId()
        {
            var list = _repository.ReadListByBankId(1);
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
            Assert.IsTrue(list.TrueForAll(b=>b.Bank.BankId==1));
        }

        [TestMethod]
        public void TestReadListDataIdName()
        {
            var list = _repository.ReadListDataIdName();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);

            Assert.IsTrue(list.Count(d => d.Id > 0) == list.Count);
            Assert.IsTrue(list.Count(d => !String.IsNullOrEmpty(d.Name)) == list.Count);
        }


        private void CompareBankAccounts(Core.Entities.BankAccount entity1, Core.Entities.BankAccount entity2, string prefix)
        {
            Assert.IsNotNull(entity1, "{0} - entity1", prefix);
            Assert.IsNotNull(entity2, "{0} - entity2", prefix);

            Assert.AreEqual(entity1.BankAccountId, entity2.BankAccountId, "{0} - BankAccountId", prefix);
            Assert.AreEqual(entity1.Name, entity2.Name, "{0} - Name", prefix);

            Assert.IsNotNull(entity1.Bank, "{0} - entity1.Bank", prefix);
            Assert.IsNotNull(entity2.Bank, "{0} - entity2.Bank", prefix);

            Assert.AreEqual(entity1.Bank.BankId, entity2.Bank.BankId, "{0} - BankId", prefix);
            Assert.AreEqual(entity1.SortCode, entity2.SortCode, "{0} - SortCode", prefix);
            Assert.AreEqual(entity1.AccountNumber, entity2.AccountNumber, "{0} - AccountNumber", prefix);
            Assert.AreEqual(entity1.AccountOwner, entity2.AccountOwner, "{0} - AccountOwner", prefix);
            Assert.AreEqual(entity1.PaysTaxableInterest, entity2.PaysTaxableInterest, "{0} - PaysTaxableInterest", prefix);
            Assert.AreEqual(entity1.LoginURL, entity2.LoginURL, "{0} - LoginURL", prefix);
            Assert.AreEqual(entity1.LoginID, entity2.LoginID, "{0} - LoginID", prefix);
            Assert.AreEqual(entity1.PasswordHint, entity2.PasswordHint, "{0} - PasswordHint", prefix);
            Assert.AreEqual(entity1.OpenedDate, entity2.OpenedDate, "{0} - OpenedDate", prefix);
            Assert.AreEqual(entity1.ClosedDate, entity2.ClosedDate, "{0} - ClosedDate", prefix);
            Assert.AreEqual(entity1.InitialRate, entity2.InitialRate, "{0} - InitialRate", prefix);
            Assert.AreEqual(entity1.MilestoneDate, entity2.MilestoneDate, "{0} - MilestoneDate", prefix);
            Assert.AreEqual(entity1.MilestoneNotes, entity2.MilestoneNotes, "{0} - MilestoneNotes", prefix);
            Assert.AreEqual(entity1.Notes, entity2.Notes, "{0} - Notes", prefix);
        }




    }
}
