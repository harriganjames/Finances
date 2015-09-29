using System;
using Finances.Core.Interfaces;
using Finances.Persistence.EF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using System.Linq;
using Finances.Core.Factories;
using Finances.Core.Engines;
using Finances.Core.Entities;
using Castle.Windsor;

namespace Finances.IntegrationTests.MS
{
    [TestClass]
    public class TransferRepositoryEFTests : IntegrationBase
    {
        ITransferRepository repository;
        Core.Entities.Transfer testEntity;
        ITransferFactory transferFactory;

        [TestInitialize]
        public void Initialize()
        {

            repository = container.Resolve<ITransferRepository>();

            transferFactory = container.Resolve<ITransferFactory>();

            testEntity = transferFactory.Create();

            testEntity.Name = "xfer-" + Guid.NewGuid().ToString();
            testEntity.Amount = 123;
            testEntity.AmountTolerence = 0.5M;
            testEntity.Schedule.StartDate = DateTime.Now.Date;
            testEntity.Schedule.EndDate = DateTime.Now.AddDays(30).Date;
            testEntity.Schedule.Frequency = "Monthly";
            testEntity.IsEnabled = true;
            testEntity.FromBankAccount = new Core.Entities.BankAccount()
            {
                BankAccountId = 1
            };
            testEntity.ToBankAccount = new Core.Entities.BankAccount()
            {
                BankAccountId = 2
            };
            testEntity.Category = new Core.Entities.TransferCategory()
            {
                TransferCategoryId = 1
            };



        }


        [TestMethod]
        public void TestCRUD()
        {
            Core.Entities.Transfer read;
            var entity = testEntity;

            repository.Add(entity);
            Assert.IsTrue(entity.TransferId > 0, "TransferId not set");

            read = repository.Read(entity.TransferId);
            Assert.IsNotNull(read);
            Assert.IsNotNull(read.FromBankAccount);
            Assert.IsNotNull(read.FromBankAccount.Name);
            Assert.IsNotNull(read.FromBankAccount.Bank);
            Assert.IsNotNull(read.FromBankAccount.Bank.Name);
            Assert.IsNotNull(read.ToBankAccount);
            Assert.IsNotNull(read.ToBankAccount.Name);
            Assert.IsNotNull(read.ToBankAccount.Bank);
            Assert.IsNotNull(read.ToBankAccount.Bank.Name);
            Assert.IsNotNull(read.Category);
            Assert.IsNotNull(read.Category.Name);
            
            CompareTransfers(entity, read, "Read");

            entity.Name += "-UPDATE";
            repository.Update(entity);
            read = repository.Read(entity.TransferId);
            Assert.IsNotNull(read);
            CompareTransfers(entity, read, "Update");

            repository.Delete(entity);

            read = repository.Read(entity.TransferId);
            Assert.IsNull(read);
        }


        [TestMethod]
        public void TestReadList()
        {
            var list = repository.ReadList();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
            Assert.IsNotNull(list[0].Category);
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

        [TestMethod]
        public void TestReadListTransferCategory()
        {
            var list = repository.ReadListTransferCategories();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);

            Assert.IsTrue(list.Count(d => d.TransferCategoryId >= -1) == list.Count);
            Assert.IsTrue(list.Count(d => !String.IsNullOrEmpty(d.Code)) == list.Count);
            Assert.IsTrue(list.Count(d => !String.IsNullOrEmpty(d.Name)) == list.Count);
        }


        private void CompareTransfers(Core.Entities.Transfer entity1, Core.Entities.Transfer entity2, string prefix)
        {
            Assert.IsNotNull(entity1, "{0} - entity1", prefix);
            Assert.IsNotNull(entity2, "{0} - entity2", prefix);

            Assert.AreEqual(entity1.Name, entity2.Name, "{0} - Name", prefix);
            Assert.AreEqual(entity1.Amount, entity2.Amount, "{0} - Amount", prefix);
            Assert.AreEqual(entity1.AmountTolerence, entity2.AmountTolerence, "{0} - AmountTolerence", prefix);
            Assert.AreEqual(entity1.Schedule.EndDate, entity2.Schedule.EndDate, "{0} - EndDate", prefix);
            Assert.AreEqual(entity1.Schedule.StartDate, entity2.Schedule.StartDate, "{0} - StartDate", prefix);
            Assert.AreEqual(entity1.Schedule.Frequency, entity2.Schedule.Frequency, "{0} - Frequency", prefix);
            Assert.AreEqual(entity1.IsEnabled, entity2.IsEnabled, "{0} - IsEnabled", prefix);

            Assert.IsNotNull(entity1.FromBankAccount, "{0} - entity1.FromBankAccount", prefix);
            Assert.IsNotNull(entity2.FromBankAccount, "{0} - entity2.FromBankAccount", prefix);

            Assert.IsNotNull(entity1.ToBankAccount, "{0} - entity1.ToBankAccount", prefix);
            Assert.IsNotNull(entity2.ToBankAccount, "{0} - entity2.ToBankAccount", prefix);

            Assert.IsNotNull(entity1.FromBankAccount.Bank, "{0} - entity1.FromBankAccount.Bank", prefix);
            Assert.IsNotNull(entity2.FromBankAccount.Bank, "{0} - entity2.FromBankAccount.Bank", prefix);

            Assert.IsNotNull(entity1.ToBankAccount.Bank, "{0} - entity1.ToBankAccount.Bank", prefix);
            Assert.IsNotNull(entity2.ToBankAccount.Bank, "{0} - entity2.ToBankAccount.Bank", prefix);

            Assert.AreEqual(entity1.FromBankAccount.BankAccountId, entity2.FromBankAccount.BankAccountId, "{0} - FromBankAccount", prefix);
            Assert.AreEqual(entity1.ToBankAccount.BankAccountId, entity2.ToBankAccount.BankAccountId, "{0} - ToBankAccount", prefix);

            Assert.IsNotNull(entity1.Category, "{0} - entity1.Category", prefix);
            Assert.IsNotNull(entity2.Category, "{0} - entity2.Category", prefix);

            Assert.AreEqual(entity1.Category.TransferCategoryId, entity2.Category.TransferCategoryId, "{0} - Category", prefix);


            //CompareBankAccounts(entity1.FromBankAccount, entity2.FromBankAccount, String.Format("{0} - FromBankAccount", prefix));
            //CompareBankAccounts(entity1.ToBankAccount, entity2.ToBankAccount, String.Format("{0} - ToBankAccount", prefix));
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
