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
using Castle.MicroKernel.Registration;
using Finances.Core.WindsorInstallers;
using Finances.Service;
using Finances.Persistence.EF.Mappings;
using Castle.Windsor.Installer;
using Finances.Core;
using Finances.Interface;
using System.Collections.Generic;

namespace Finances.IntegrationTests.MS
{
    [TestClass]
    public class BalanceDateRepositoryEFTests : IntegrationBase
    {
        IBalanceDateRepository repository;
        Core.Entities.BalanceDate testEntity;
        IBalanceDateFactory transferFactory;

        [TestInitialize]
        public void Initialize()
        {

            repository = container.Resolve<IBalanceDateRepository>();

            transferFactory = container.Resolve<IBalanceDateFactory>();

            testEntity = transferFactory.Create();

            testEntity.DateOfBalance = DateTime.Today;
            testEntity.BalanceDateBankAccounts.AddRange(new List<Core.Entities.BalanceDateBankAccount>()
            {
                    new Core.Entities.BalanceDateBankAccount()
                    {
                        BankAccount = new Core.Entities.BankAccount() { BankAccountId=1 },
                        BalanceAmount = 123
                    },
                    new Core.Entities.BalanceDateBankAccount()
                    {
                        BankAccount = new Core.Entities.BankAccount() { BankAccountId=2 },
                        BalanceAmount = 456
                    },
            });


        }


        [TestMethod]
        public void TestCRUD()
        {
            // clear out first
            var all = repository.ReadListDataIdName();
            repository.Delete(all.Select(d => d.Id).ToList());
            //

            Core.Entities.BalanceDate read;
            var entity = testEntity;

            repository.Add(entity);
            Assert.IsTrue(entity.BalanceDateId > 0, "BalanceDateId not set");

            read = repository.Read(entity.BalanceDateId);
            Assert.IsNotNull(read);
            Assert.IsNotNull(read.BalanceDateId);
            Assert.IsNotNull(read.DateOfBalance);
            Assert.IsNotNull(read.RecordCreatedDateTime);

            CompareBalanceDates(entity, read, "Read");

            entity.DateOfBalance = entity.DateOfBalance.AddDays(1);
            repository.Update(entity);
            read = repository.Read(entity.BalanceDateId);
            Assert.IsNotNull(read);
            CompareBalanceDates(entity, read, "Update");

            repository.Delete(entity);

            read = repository.Read(entity.BalanceDateId);
            Assert.IsNull(read);
        }


        [TestMethod]
        public void TestReadList()
        {
            var list = repository.ReadList();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
            Assert.IsNotNull(list[0].DateOfBalance);
            Assert.IsNotNull(list[0].BalanceDateBankAccounts);
            Assert.IsTrue(list[0].BalanceDateBankAccounts.Count>0);
            Assert.IsNotNull(list[0].BalanceDateBankAccounts[0].BankAccount);
            Assert.IsNotNull(list[0].BalanceDateBankAccounts[0].BankAccount.Bank);
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




        private void CompareBalanceDates(Core.Entities.BalanceDate entity1, Core.Entities.BalanceDate entity2, string prefix)
        {
            Assert.IsNotNull(entity1, "{0} - entity1", prefix);
            Assert.IsNotNull(entity2, "{0} - entity2", prefix);

            Assert.AreEqual(entity1.DateOfBalance, entity2.DateOfBalance, "{0} - DateOfBalance", prefix);

            Assert.AreEqual(entity1.BalanceDateBankAccounts.Count, entity2.BalanceDateBankAccounts.Count, "{0} - BalanceDateBankAccounts.Count", prefix);

            for (int i = 0; i < entity1.BalanceDateBankAccounts.Count; i++)
            {
                CompareBalanceDateBankAccounts(entity1.BalanceDateBankAccounts[i], entity2.BalanceDateBankAccounts[i], String.Format("{0} - BalanceDateBankAccounts[{1}]", prefix, i));
            }

        }

        private void CompareBalanceDateBankAccounts(Core.Entities.BalanceDateBankAccount entity1, Core.Entities.BalanceDateBankAccount entity2, string prefix)
        {
            Assert.IsNotNull(entity1, "{0} - entity1", prefix);
            Assert.IsNotNull(entity2, "{0} - entity2", prefix);

            Assert.IsNotNull(entity1.BankAccount, "{0} - entity1.BankAccount", prefix);
            Assert.IsNotNull(entity2.BankAccount, "{0} - entity2.BankAccount", prefix);

            Assert.AreEqual(entity1.BankAccount.BankAccountId, entity2.BankAccount.BankAccountId, "{0} - BankAccountId", prefix);

            Assert.AreEqual(entity1.BalanceAmount, entity2.BalanceAmount, "{0} - BalanceAmount", prefix);
        }




    }
}
