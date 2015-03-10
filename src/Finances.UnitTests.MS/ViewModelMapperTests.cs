using System;
using System.Linq;
using System.Collections.Generic;
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
        Bank testBankEntity2;
        BankAccount testBankAccountEntity;
        Transfer testTransferEntity;
        Cashflow testCashflowEntity;

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

            testBankEntity2 = new Bank()
            {
                BankId = 2,
                Name = "fd",
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

            testTransferEntity = new Core.Entities.Transfer()
            {
                TransferId = 1,
                Name = "test transfer",
                FromBankAccount = new BankAccount() { BankAccountId = 1 },
                ToBankAccount = new BankAccount() { BankAccountId = 2 },
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date.AddDays(1),
                Amount = 1234M,
                AmountTolerence = 0.01M,
                Frequency = "Monthly",
                IsEnabled = true
            };


            testCashflowEntity = new Core.Entities.Cashflow()
            {
                CashflowId = 1,
                Name = "test cashflow",
                OpeningBalance = 123M,
                StartDate = DateTime.Now.Date,
                CashflowBankAccounts = new List<Core.Entities.CashflowBankAccount>() { 
                    new Core.Entities.CashflowBankAccount()
                        {
                            CashflowBankAccountId = 1,
                            BankAccount = testBankAccountEntity
                        }
                    }
            };



        }


        //[TestMethod]
        //public void Test_Map_InputDecimal_to_decimal_And_Reverse()
        //{
        //    var ind = new InputDecimal() { Value = 123M };

        //    var dec = Mapper.Map<decimal>(ind);

        //    Assert.AreEqual(dec, ind.Value);

        //    var newind = Mapper.Map<InputDecimal>(dec);

        //    Assert.AreEqual(dec, newind.Value);
        //}



        // Bank -> BankItemViewModel
        // Bank -> BankItemViewModel
        // BankItemViewModel -> Bank
        // BankItemViewModel -> Bank


        //[TestMethod]
        //public void Test_AutoMapper_ConfigurationIsValid()
        //{
        //    mapper.ConfigurationProvider.AssertConfigurationIsValid();
        //}


        //
        // Banks
        //

        [TestMethod]
        public void Test_Map_Bank_to_BankItemViewModel_And_Reverse()
        {
            var bvm = mapper.Map<BankItemViewModel>(testBankEntity);

            var b = mapper.Map<Bank>(bvm);

            CompareBanks(testBankEntity, b, "Test_Map_Bank_to_BankItemViewModel_And_Reverse");

        }

        [TestMethod]
        public void Test_Map_Bank_to_BankEditorViewModel_And_Reverse()
        {
            BankEditorViewModel editor = new BankEditorViewModel(null, null);

            mapper.Map<Bank,BankEditorViewModel>(testBankEntity,editor);

            var b = mapper.Map<Bank>(editor);

            CompareBanks(testBankEntity, b, "Test_Map_Bank_to_BankEditorViewModel_And_Reverse");
        }


        [TestMethod]
        public void Test_Map_BankList_to_BankItemViewModelList()
        {
            List<BankItemViewModel> vms;

            var entities = new List<Bank>() { testBankEntity, testBankEntity2 };

            vms = mapper.Map<List<BankItemViewModel>>(entities);

            Assert.IsNotNull(vms);

            Assert.AreEqual(entities.Count, vms.Count);

        }


        //
        // BankAccounts
        //

        [TestMethod]
        public void Test_Map_BankAccount_to_BankAccountItemViewModel_And_Reverse()
        {
            var bavm = mapper.Map<BankAccountItemViewModel>(testBankAccountEntity);

            var ba = mapper.Map<BankAccount>(bavm);

            CompareBankAccounts(testBankAccountEntity, ba, "Test_Map_BankAccount_to_BankAccountItemViewModel_And_Reverse");

        }

        [TestMethod]
        public void Test_Map_BankAccount_to_BankAccountEditorViewModel_And_Reverse()
        {
            BankAccountEditorViewModel editor = new BankAccountEditorViewModel(null,null,null,null,null);

            mapper.Map<BankAccount, BankAccountEditorViewModel>(testBankAccountEntity, editor);

            var ba = mapper.Map<BankAccount>(editor);

            CompareBankAccounts(testBankAccountEntity, ba, "Test_Map_BankAccount_to_BankAccountEditorViewModel_And_Reverse");

        }

        //
        // Transfers
        //

        [TestMethod]
        public void Test_Map_Transfer_to_TransferItemViewModel_And_Reverse()
        {
            var bvm = mapper.Map<TransferItemViewModel>(testTransferEntity);

            var b = mapper.Map<Transfer>(bvm);

            CompareTransfers(testTransferEntity, b, "Test_Map_Transfer_to_TransferItemViewModel_And_Reverse");

        }

        [TestMethod]
        public void Test_Map_Transfer_to_TransferEditorViewModel_And_Reverse()
        {
            TransferEditorViewModel editor = new TransferEditorViewModel(null, null, null, null, null);

            mapper.Map<Transfer, TransferEditorViewModel>(testTransferEntity, editor);

            var b = mapper.Map<Transfer>(editor);

            CompareTransfers(testTransferEntity, b, "Test_Map_Transfer_to_TransferEditorViewModel_And_Reverse");
        }

        [TestMethod]
        public void Test_Map_Transfer_to_TransferEditorViewModel_Optional_BankAccount_And_Reverse()
        {
            TransferEditorViewModel editor = new TransferEditorViewModel(null, null, null, null, null);

            testTransferEntity.FromBankAccount = null;

            mapper.Map<Transfer, TransferEditorViewModel>(testTransferEntity, editor);

            var b = mapper.Map<Transfer>(editor);

            CompareTransfers(testTransferEntity, b, "Test_Map_Transfer_to_TransferEditorViewModel_Optional_BankAccount_And_Reverse");

            testTransferEntity.ToBankAccount = null;

            mapper.Map<Transfer, TransferEditorViewModel>(testTransferEntity, editor);

            b = mapper.Map<Transfer>(editor);

            CompareTransfers(testTransferEntity, b, "Test_Map_Transfer_to_TransferEditorViewModel_Optional_BankAccount_And_Reverse");
        }


        [TestMethod]
        public void Test_Map_Transfer_to_Editor_Elsewhere()
        {
            // null BankAccounts in Transfer should convert to BankAccountItemViewModel.Elsewhere in editor and vice-versa
            //
            TransferEditorViewModel editor = new TransferEditorViewModel(null, null, null, null, null);

            testTransferEntity.FromBankAccount = null;
            testTransferEntity.ToBankAccount = null;

            mapper.Map<Transfer, TransferEditorViewModel>(testTransferEntity, editor);

            Assert.IsNotNull(editor.FromBankAccount);
            Assert.IsNotNull(editor.ToBankAccount);

            Assert.AreEqual(editor.FromBankAccount.BankAccountId, BankAccountItemViewModel.Elsewhere.BankAccountId);
            Assert.AreEqual(editor.ToBankAccount.BankAccountId, BankAccountItemViewModel.Elsewhere.BankAccountId);

            var t = mapper.Map<Transfer>(editor);

            Assert.IsNull(t.FromBankAccount);
            Assert.IsNull(t.ToBankAccount);

        }

        [TestMethod]
        public void Test_Map_Cashflow_to_CashflowItemViewModel_And_Reverse()
        {
            var vm = mapper.Map<CashflowItemViewModel>(testCashflowEntity);

            var e = mapper.Map<Cashflow>(vm);

            CompareCashflows(testCashflowEntity, e, "Test_Map_Cashflow_to_CashflowItemViewModel_And_Reverse");

        }

        [TestMethod]
        public void Test_Map_Cashflow_to_CashflowItemViewModel_NoAccounts_And_Reverse()
        {
            testCashflowEntity.CashflowBankAccounts.Clear();

            var vm = mapper.Map<CashflowItemViewModel>(testCashflowEntity);

            Assert.IsTrue(vm.CashflowBankAccounts.Count == 0, "vm.CashflowBankAccounts.Count == 0");

            var e = mapper.Map<Cashflow>(vm);

            CompareCashflows(testCashflowEntity, e, "Test_Map_Cashflow_to_CashflowItemViewModel_NoAccounts_And_Reverse");

        }


        //maybe use after/before map to populate collection?


        [TestMethod]
        public void Test_Map_Cashflow_to_CashflowEditorViewModel_And_Reverse()
        {
            CashflowEditorViewModel editor = new CashflowEditorViewModel(null, null, null, null);

            mapper.Map<Cashflow, CashflowEditorViewModel>(testCashflowEntity, editor);

            var b = mapper.Map<Cashflow>(editor);

            CompareCashflows(testCashflowEntity, b, "Test_Map_Cashflow_to_CashflowEditorViewModel_And_Reverse");
        }


        #region Privates

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

        private void CompareTransfers(Core.Entities.Transfer entity1, Core.Entities.Transfer entity2, string prefix)
        {
            Assert.IsNotNull(entity1, "{0} - entity1", prefix);
            Assert.IsNotNull(entity2, "{0} - entity2", prefix);

            Assert.AreEqual(entity1.TransferId, entity2.TransferId, "{0} - TransferId", prefix);
            Assert.AreEqual(entity1.Name, entity2.Name, "{0} - Name", prefix);

            Assert.IsTrue((entity1.FromBankAccount == null && entity2.FromBankAccount == null) || (entity1.FromBankAccount != null && entity2.FromBankAccount != null), "{0} - FromBankAccount null check", prefix);
            Assert.IsTrue((entity1.ToBankAccount == null && entity2.ToBankAccount == null) || (entity1.ToBankAccount != null && entity2.ToBankAccount != null),"{0} - FromBankAccount null check", prefix);

            //Assert.IsNotNull(entity1.FromBankAccount, "{0} - entity1.FromBankAccount", prefix);
            //Assert.IsNotNull(entity2.FromBankAccount, "{0} - entity2.FromBankAccount", prefix);
            //Assert.IsNotNull(entity1.ToBankAccount, "{0} - entity1.ToBankAccount", prefix);
            //Assert.IsNotNull(entity2.ToBankAccount, "{0} - entity2.ToBankAccount", prefix);

            if(entity1.FromBankAccount!=null)
                Assert.AreEqual(entity1.FromBankAccount.BankAccountId, entity2.FromBankAccount.BankAccountId, "{0} - FromBankAccount.BankAccountId", prefix);
            if(entity1.ToBankAccount!=null)
                Assert.AreEqual(entity1.ToBankAccount.BankAccountId, entity2.ToBankAccount.BankAccountId, "{0} - ToBankAccount.BankAccountId", prefix);

            Assert.AreEqual(entity1.AmountTolerence, entity2.AmountTolerence, "{0} - AmountTolerence", prefix);
            Assert.AreEqual(entity1.Amount, entity2.Amount, "{0} - Amount", prefix);
            Assert.AreEqual(entity1.StartDate, entity2.StartDate, "{0} - StartDate", prefix);
            Assert.AreEqual(entity1.EndDate, entity2.EndDate, "{0} - EndDate", prefix);
            Assert.AreEqual(entity1.Frequency, entity2.Frequency, "{0} - Frequency", prefix);
            Assert.AreEqual(entity1.IsEnabled, entity2.IsEnabled, "{0} - IsEnabled", prefix);
        }


        private void CompareCashflows(Core.Entities.Cashflow entity1, Core.Entities.Cashflow entity2, string prefix)
        {
            Assert.AreEqual(entity1.CashflowId, entity2.CashflowId, "{0} - CashflowId");
            Assert.AreEqual(entity1.Name, entity2.Name, "{0} - Cashflow Name");
            Assert.AreEqual(entity1.OpeningBalance, entity2.OpeningBalance, "{0} - OpeningBalance");
            Assert.AreEqual(entity1.StartDate, entity2.StartDate, "{0} - StartDate");
            Assert.AreEqual(entity1.RecordCreatedDateTime, entity2.RecordCreatedDateTime, "{0} - RecordCreatedDateTime");
            Assert.AreEqual(entity1.RecordUpdatedDateTime, entity2.RecordUpdatedDateTime, "{0} - RecordUpdatedDateTime");

            Assert.IsNotNull(entity1.CashflowBankAccounts, "{0} - entity1 CashflowBankAccounts null");
            Assert.IsNotNull(entity2.CashflowBankAccounts, "{0} - entity2 CashflowBankAccounts null");
            Assert.AreEqual(entity1.CashflowBankAccounts.Count, entity2.CashflowBankAccounts.Count, "{0} - CashflowBankAccounts qty");

            foreach (var e1cba in entity1.CashflowBankAccounts)
            {
                Assert.IsTrue(e1cba.BankAccount.BankAccountId > 0, "{0} - e1cba.BankAccount.BankAccountId > 0");
                var e2cba = entity2.CashflowBankAccounts.FirstOrDefault(o => o.BankAccount.BankAccountId == e1cba.BankAccount.BankAccountId);
                Assert.IsNotNull(e2cba, "{0} - entity1 BankAccount not found in entity2");

                CompareCashflowBankAccounts(e1cba, e2cba, prefix);
            }
        }

        private void CompareCashflowBankAccounts(Core.Entities.CashflowBankAccount entity1, Core.Entities.CashflowBankAccount entity2, string prefix)
        {
            Assert.AreEqual(entity1.CashflowBankAccountId, entity2.CashflowBankAccountId, "{0} - CashflowBankAccountId");
            CompareBankAccounts(entity1.BankAccount, entity2.BankAccount, prefix);
            Assert.AreEqual(entity1.RecordCreatedDateTime, entity2.RecordCreatedDateTime, "{0} - cba RecordCreatedDateTime");
        }



        #endregion
    }
}
