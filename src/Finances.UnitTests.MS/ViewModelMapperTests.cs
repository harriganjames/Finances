using System;
using AutoMapper;
using Finances.Core.Entities;
using Finances.WinClient.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Finances_WinClient = Finances.WinClient;

//using Finances.WinClient;

namespace Finances.UnitTests.MS
{
    [TestClass]
    public class ViewModelMapperTests
    {
        IMappingEngine mapper;
        Bank testBankEntity;
        BankAccount testBankAccountEntity;

        [TestInitialize]
        public void Initialize()
        {
            new Finances.WinClient.DomainServices.MappingCreator().CreateMappings();

            mapper = Mapper.Engine;

            testBankEntity = new Bank()
            {
                BankId = 1,
                Name = "HSVBC",
                Logo = new byte[] { 1, 2, 3 }
            };

            testBankAccountEntity = new Core.Entities.BankAccount()
            {
                BankAccountId = 1,
                Bank = testBankEntity,
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


        // Bank -> BankItemViewModel
        // Bank -> IBankItemViewModel
        // BankItemViewModel -> Bank
        // IBankItemViewModel -> Bank


        [TestMethod]
        public void Test_AutoMapper_ConfigurationIsValid()
        {
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void Test_Map_Bank_to_BankItemViewModel_And_Reverse()
        {
            var bvm = mapper.Map<BankItemViewModel>(testBankEntity);

            var b = mapper.Map<Bank>(bvm);

            CompareBanks(testBankEntity, b, "Test_Map_Bank_to_BankItemViewModel_And_Reverse");

        }

        [TestMethod]
        public void Test_Map_BankAccount_to_BankAccountItemViewModel_And_Reverse()
        {
            var bavm = mapper.Map<BankAccountItemViewModel>(testBankAccountEntity);

            var ba = mapper.Map<BankAccount>(bavm);

            CompareBankAccounts(testBankAccountEntity, ba, "Test_Map_BankAccount_to_BankAccountItemViewModel_And_Reverse");

        }

        private void CompareBanks(Core.Entities.Bank entity1, Core.Entities.Bank entity2, string prefix)
        {
            Assert.IsNotNull(entity1, "{0} - entity1", prefix);
            Assert.IsNotNull(entity2, "{0} - entity2", prefix);

            Assert.AreEqual(entity1.BankId, entity2.BankId, "{0} - BankId", prefix);
            Assert.AreEqual(entity1.Name, entity2.Name, "{0} - Name", prefix);
            Assert.AreEqual(entity1.Logo, entity2.Logo, "{0} - Logo", prefix);
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
