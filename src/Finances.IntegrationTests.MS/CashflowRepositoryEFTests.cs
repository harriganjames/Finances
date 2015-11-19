using System;
using Finances.Core.Interfaces;
using Finances.Persistence.EF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using Finances.Core.Factories;


namespace Finances.IntegrationTests.MS
{
    [TestClass]
    public class CashflowRepositoryEFTests : IntegrationBase
    {
        IRepositoryRead<Core.Entities.Cashflow> repositoryRead;
        IRepositoryWrite<Core.Entities.Cashflow> repositoryWrite;
        Core.Entities.Cashflow testEntity;

        [TestInitialize]
        public void Initialize()
        {
            repositoryRead = container.Resolve<IRepositoryRead<Core.Entities.Cashflow>>();
            repositoryWrite = container.Resolve<IRepositoryWrite<Core.Entities.Cashflow>>();

            testEntity = new Core.Entities.Cashflow(null)
            {
                Name = "cflow-" + Guid.NewGuid().ToString(),
                OpeningBalance = 123M,
                StartDate = DateTime.Now.Date,
                CashflowBankAccounts = new List<Core.Entities.CashflowBankAccount>() { 
                        new Core.Entities.CashflowBankAccount() { BankAccount=new Core.Entities.BankAccount() { BankAccountId=1 } },
                        new Core.Entities.CashflowBankAccount() { BankAccount=new Core.Entities.BankAccount() { BankAccountId=2 } }
                            }
            };

        }


        [TestMethod]
        public void TestCRUD()
        {
            Core.Entities.Cashflow read;
            var entity = testEntity;

            repositoryWrite.Add(entity);
            Assert.IsTrue(entity.CashflowId > 0, "CashflowId not set");

            read = repositoryRead.Read(entity.CashflowId);
            Assert.IsNotNull(read);
            Assert.IsNotNull(read.CashflowBankAccounts);
            
            CompareCashflows(entity, read, "Add-Read");

            entity.Name += "-UPDATE";
            entity.CashflowBankAccounts.Remove(entity.CashflowBankAccounts[0]);
            entity.CashflowBankAccounts.Add(new Core.Entities.CashflowBankAccount() { BankAccount = new Core.Entities.BankAccount() { BankAccountId = 3 } });

            repositoryWrite.Update(entity);
            read = repositoryRead.Read(entity.CashflowId);
            Assert.IsNotNull(read);
            CompareCashflows(entity, read, "Update");

            repositoryWrite.Delete(entity);

            read = repositoryRead.Read(entity.CashflowId);
            Assert.IsNull(read);
        }


        [TestMethod]
        public void TestReadList()
        {
            var list = repositoryRead.ReadList();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);

            //check children are populated

            foreach (var c in list)
            {
                Assert.IsNotNull(c.CashflowBankAccounts);
                foreach (var cba in c.CashflowBankAccounts)
                {
                    Assert.IsNotNull(cba.BankAccount);
                    Assert.IsTrue(cba.BankAccount.BankAccountId > 0);

                    Assert.IsNotNull(cba.BankAccount.Bank);
                    Assert.IsTrue(cba.BankAccount.Bank.BankId > 0);
                }
            }

        }


        [TestMethod]
        public void TestReadListDataIdName()
        {
            var list = repositoryRead.ReadListDataIdName();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);

            Assert.IsTrue(list.Count(d => d.Id > 0) == list.Count);
            Assert.IsTrue(list.Count(d => !String.IsNullOrEmpty(d.Name)) == list.Count);
        }


        private void CompareCashflows(Core.Entities.Cashflow entity1, Core.Entities.Cashflow entity2, string prefix)
        {
            Assert.IsNotNull(entity1, "{0} - entity1", prefix);
            Assert.IsNotNull(entity2, "{0} - entity2", prefix);

            Assert.AreEqual(entity1.CashflowId, entity2.CashflowId, "CashflowId");
            Assert.AreEqual(entity1.Name, entity2.Name, "Cashflow Name");
            Assert.AreEqual(entity1.OpeningBalance, entity2.OpeningBalance, "OpeningBalance");
            Assert.AreEqual(entity1.StartDate, entity2.StartDate, "StartDate");
            Assert.AreEqual(entity1.RecordCreatedDateTime, entity2.RecordCreatedDateTime, "RecordCreatedDateTime");
            Assert.AreEqual(entity1.RecordUpdatedDateTime, entity2.RecordUpdatedDateTime, "RecordUpdatedDateTime");

            Assert.IsNotNull(entity1.CashflowBankAccounts, "entity1 CashflowBankAccounts null");
            Assert.IsNotNull(entity2.CashflowBankAccounts, "entity2 CashflowBankAccounts null");
            Assert.AreEqual(entity1.CashflowBankAccounts.Count, entity2.CashflowBankAccounts.Count, "CashflowBankAccounts qty");

            foreach (var e1cba in entity1.CashflowBankAccounts)
            {
                Assert.IsTrue(e1cba.BankAccount.BankAccountId > 0, "e1cba.BankAccount.BankAccountId > 0");
                var e2cba = entity2.CashflowBankAccounts.FirstOrDefault(o => o.BankAccount.BankAccountId == e1cba.BankAccount.BankAccountId);
                Assert.IsNotNull(e2cba, "entity1 BankAccount not found in entity2");

                CompareCashflowBankAccounts(e1cba, e2cba);
            }

       }


        private void CompareCashflowBankAccounts(Core.Entities.CashflowBankAccount entity1, Core.Entities.CashflowBankAccount entity2)
        {
            Assert.AreEqual(entity1.CashflowBankAccountId, entity2.CashflowBankAccountId, "CashflowBankAccountId");
            Assert.AreEqual(entity1.BankAccount.BankAccountId, entity2.BankAccount.BankAccountId, "cba BankAccountId");
            Assert.AreEqual(entity1.RecordCreatedDateTime, entity2.RecordCreatedDateTime, "cba RecordCreatedDateTime");
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
