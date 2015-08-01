using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Finances_Persistence = Finances.Persistence;

namespace Finances.UnitTests.MS
{
    //[TestClass]
    //public class OrmEntityMapperTests
    //{
    //    IMappingEngine _mapper;

    //    [TestInitialize]
    //    public void Initialize()
    //    {

    //        new Finances.Persistence.EF.MappingCreator().CreateMappings();

    //        _mapper = Mapper.Engine;
    //    }


    //    [TestMethod]
    //    public void Test_Bank_EntityToOrm()
    //    {
    //        var entity = new Core.Entities.Bank() { BankId=1, Name="one", Logo=new byte[] { 1, 2, 3 } };

    //        var orm = _mapper.Map<Finances.Persistence.EF.Bank>(entity);


    //        CompareBanks(entity, orm);
    //    }

    //    [TestMethod]
    //    public void Test_Bank_OrmToEntity()
    //    {
    //        var orm = new Finances.Persistence.EF.Bank() { BankId = 1, Name = "one", Logo = new byte[] { 1, 2, 3 } };

    //        var entity = _mapper.Map<Core.Entities.Bank>(orm);

    //        CompareBanks(entity, orm);
    //    }


    //    private void CompareBanks(Core.Entities.Bank entity, Finances.Persistence.EF.Bank orm)
    //    {
    //        Assert.AreEqual(entity.BankId, orm.BankId, "BankId");
    //        Assert.AreEqual(entity.Name, orm.Name, "BankName");
    //        Assert.AreEqual(entity.Logo, orm.Logo, "Logo");
    //    }


    //    [TestMethod]
    //    public void Test_BankAccount_EntityToOrm()
    //    {
    //        var entity = new Core.Entities.BankAccount()
    //        {
    //            BankAccountId = 1,
    //            Bank = new Core.Entities.Bank() { BankId = 1, Name = "one", Logo = new byte[] { 1, 2, 3 } },
    //            AccountNumber = "1234",
    //            AccountOwner = "me",
    //            Name = "savings",
    //            LoginID = "login",
    //            LoginURL = "abc",
    //            PasswordHint = "hint",
    //            OpenedDate = DateTime.Now.Date,
    //            ClosedDate = DateTime.Now.Date.AddDays(1),
    //            SortCode = "00-00-10",
    //            InitialRate = 1.1M,
    //            PaysTaxableInterest = true,
    //            MilestoneDate = DateTime.Now.Date.AddDays(2),
    //            MilestoneNotes = "msnotes",
    //            Notes = "test notes"
    //        };

    //        var orm = _mapper.Map<Finances.Persistence.EF.BankAccount>(entity);

    //        CompareBankAccounts(entity, orm, false);
    //    }


    //    [TestMethod]
    //    public void Test_BankAccount_OrmToEntity()
    //    {
    //        var orm = new Finances.Persistence.EF.BankAccount()
    //        {
    //            BankAccountId = 1,
    //            Bank = new Finances.Persistence.EF.Bank() { BankId = 1, Name = "one", Logo = new byte[] { 1, 2, 3 } },
    //            AccountNumber = "1234",
    //            AccountOwner = "me",
    //            Name = "savings",
    //            LoginID = "login",
    //            LoginURL = "abc",
    //            PasswordHint = "hint",
    //            OpenedDate = DateTime.Now.Date,
    //            ClosedDate = DateTime.Now.Date.AddDays(1),
    //            SortCode = "00-00-10",
    //            InitialRate = 1.1M,
    //            PaysTaxableInterest = true,
    //            MilestoneDate = DateTime.Now.Date.AddDays(2),
    //            MilestoneNotes = "msnotes",
    //            Notes = "test notes"
    //        };

    //        var entity = _mapper.Map<Core.Entities.BankAccount>(orm);

    //        CompareBankAccounts(entity, orm, true);
    //    }


        
    //    private void CompareBankAccounts(Core.Entities.BankAccount entity, Finances.Persistence.EF.BankAccount orm, bool isToEntity)
    //    {
    //        Assert.AreEqual(entity.BankAccountId, orm.BankAccountId, "BankAccountId");
    //        Assert.AreEqual(entity.Name, orm.Name, "Name");
            
    //        if(isToEntity)
    //            CompareBanks(entity.Bank, orm.Bank);
    //        else
    //            Assert.AreEqual(entity.Bank.BankId, orm.BankId, "BankId");

    //        Assert.AreEqual(entity.SortCode, orm.SortCode, "SortCode");
    //        Assert.AreEqual(entity.AccountNumber, orm.AccountNumber, "AccountNumber");
    //        Assert.AreEqual(entity.AccountOwner, orm.AccountOwner, "AccountOwner");
    //        Assert.AreEqual(entity.PaysTaxableInterest, orm.PaysTaxableInterest, "PaysTaxableInterest");
    //        Assert.AreEqual(entity.LoginURL, orm.LoginURL, "LoginURL");
    //        Assert.AreEqual(entity.LoginID, orm.LoginID, "LoginID");
    //        Assert.AreEqual(entity.PasswordHint, orm.PasswordHint, "PasswordHint");
    //        Assert.AreEqual(entity.OpenedDate, orm.OpenedDate, "OpenedDate");
    //        Assert.AreEqual(entity.ClosedDate, orm.ClosedDate, "ClosedDate");
    //        Assert.AreEqual(entity.InitialRate, orm.InitialRate, "InitialRate");
    //        Assert.AreEqual(entity.MilestoneDate, orm.MilestoneDate, "MilestoneDate");
    //        Assert.AreEqual(entity.MilestoneNotes, orm.MilestoneNotes, "MilestoneNotes");
    //        Assert.AreEqual(entity.Notes, orm.Notes, "Notes");
    //    }



    //    // Cashflow


    //    [TestMethod]
    //    public void Test_Cashflow_OrmToEntity()
    //    {
    //        var account1 = new Finances.Persistence.EF.BankAccount()
    //        {
    //            BankAccountId = 1,
    //            Bank = new Finances.Persistence.EF.Bank() { BankId = 1, Name = "one", Logo = new byte[] { 1, 2, 3 } },
    //            AccountNumber = "1234",
    //            AccountOwner = "me",
    //            Name = "savings",
    //            LoginID = "login",
    //            LoginURL = "abc",
    //            PasswordHint = "hint",
    //            OpenedDate = DateTime.Now.Date,
    //            ClosedDate = DateTime.Now.Date.AddDays(1),
    //            SortCode = "00-00-10",
    //            InitialRate = 1.1M,
    //            PaysTaxableInterest = true,
    //            MilestoneDate = DateTime.Now.Date.AddDays(2),
    //            MilestoneNotes = "msnotes",
    //            Notes = "test notes"
    //        };

    //        var orm = new Finances.Persistence.EF.Cashflow()
    //        {
    //            CashflowId = 1,
    //            OpeningBalance = 123M,
    //            StartDate = DateTime.Now.Date,
    //            CashflowBankAccounts = new List<Finances.Persistence.EF.CashflowBankAccount>() { 
    //                new Finances.Persistence.EF.CashflowBankAccount()
    //                    {
    //                        CashflowBankAccountId = 1,
    //                        CashflowId = 1,
    //                        BankAccount = account1,
    //                        BankAccountId = account1.BankAccountId
    //                    }
    //                }
    //        };


    //        var entity = _mapper.Map<Core.Entities.Cashflow>(orm);

    //        CompareCashflows(entity, orm, true);
    //    }


    //    [TestMethod]
    //    public void Test_Cashflow_EntityToOrm()
    //    {
    //        var account1 = new Core.Entities.BankAccount()
    //        {
    //            BankAccountId = 1,
    //            Bank = new Core.Entities.Bank() { BankId = 1, Name = "one", Logo = new byte[] { 1, 2, 3 } },
    //            AccountNumber = "1234",
    //            AccountOwner = "me",
    //            Name = "savings",
    //            LoginID = "login",
    //            LoginURL = "abc",
    //            PasswordHint = "hint",
    //            OpenedDate = DateTime.Now.Date,
    //            ClosedDate = DateTime.Now.Date.AddDays(1),
    //            SortCode = "00-00-10",
    //            InitialRate = 1.1M,
    //            PaysTaxableInterest = true,
    //            MilestoneDate = DateTime.Now.Date.AddDays(2),
    //            MilestoneNotes = "msnotes",
    //            Notes = "test notes"
    //        };

    //        var entity = new Core.Entities.Cashflow()
    //        {
    //            CashflowId = 1,
    //            OpeningBalance = 123M,
    //            StartDate = DateTime.Now.Date,
    //            CashflowBankAccounts = new List<Core.Entities.CashflowBankAccount>() { 
    //                new Core.Entities.CashflowBankAccount()
    //                    {
    //                        CashflowBankAccountId = 1,
    //                        BankAccount = account1
    //                    }
    //                }
    //        };


    //        var orm = _mapper.Map<Persistence.EF.Cashflow>(entity);

    //        CompareCashflows(entity, orm, false);
    //    }

    //    private void CompareCashflows(Core.Entities.Cashflow entity, Finances.Persistence.EF.Cashflow orm, bool isToEntity)
    //    {
    //        Assert.AreEqual(entity.CashflowId, orm.CashflowId, "CashflowId");
    //        Assert.AreEqual(entity.Name, orm.Name, "Cashflow Name");
    //        Assert.AreEqual(entity.OpeningBalance, orm.OpeningBalance, "OpeningBalance");
    //        Assert.AreEqual(entity.StartDate, orm.StartDate, "StartDate");
    //        Assert.AreEqual(entity.RecordCreatedDateTime, orm.RecordCreatedDateTime, "RecordCreatedDateTime");
    //        Assert.AreEqual(entity.RecordUpdatedDateTime, orm.RecordUpdatedDateTime, "RecordUpdatedDateTime");

    //        Assert.IsNotNull(entity.CashflowBankAccounts, "entity CashflowBankAccounts null");
    //        Assert.IsNotNull(orm.CashflowBankAccounts, "orm CashflowBankAccounts null");
    //        Assert.AreEqual(entity.CashflowBankAccounts.Count, orm.CashflowBankAccounts.Count, "CashflowBankAccounts qty");

    //        foreach (var ecba in entity.CashflowBankAccounts)
    //        {
    //            Assert.IsTrue(ecba.BankAccount.BankAccountId > 0,"ecba.BankAccount.BankAccountId > 0");
    //            var ocba = orm.CashflowBankAccounts.FirstOrDefault(o => o.BankAccountId == ecba.BankAccount.BankAccountId);
    //            Assert.IsNotNull(ocba, "entity BankAccount not found in orm");

    //            Assert.AreEqual(entity.CashflowId, ocba.CashflowId, "CashflowBankAccount.CashflowId");

    //            CompareCashflowBankAccounts(ecba, ocba, isToEntity);
    //        }
    //    }

    //    private void CompareCashflowBankAccounts(Core.Entities.CashflowBankAccount entity, Finances.Persistence.EF.CashflowBankAccount orm, bool isToEntity)
    //    {
    //        Assert.AreEqual(entity.CashflowBankAccountId, orm.CashflowBankAccountId, "CashflowBankAccountId");
    //        if (isToEntity)
    //            CompareBankAccounts(entity.BankAccount, orm.BankAccount, isToEntity);
    //        else
    //            Assert.AreEqual(entity.BankAccount.BankAccountId, orm.BankAccountId, "cba BankAccountId");
    //        Assert.AreEqual(entity.RecordCreatedDateTime, orm.RecordCreatedDateTime, "cba RecordCreatedDateTime");
    //    }


    //    [TestMethod]
    //    public void Test_TransferCategory_EntityToOrm()
    //    {
    //        var entity = new Core.Entities.TransferCategory() { TransferCategoryId = 1, Code="a", Name = "one", DisplayOrder=1 };

    //        var orm = _mapper.Map<Finances.Persistence.EF.TransferCategory>(entity);


    //        CompareTransferCategories(entity, orm);
    //    }

    //    [TestMethod]
    //    public void Test_TransferCategory_OrmToEntity()
    //    {
    //        var orm = new Finances.Persistence.EF.TransferCategory() { TransferCategoryId = 1, Code = "a", Name = "one", DisplayOrder = 1 };

    //        var entity = _mapper.Map<Core.Entities.TransferCategory>(orm);

    //        CompareTransferCategories(entity, orm);
    //    }



    //    private void CompareTransferCategories(Core.Entities.TransferCategory entity, Finances.Persistence.EF.TransferCategory orm)
    //    {
    //        Assert.AreEqual(entity.TransferCategoryId, orm.TransferCategoryId, "TransferCategoryId");
    //        Assert.AreEqual(entity.Code, orm.Code, "Code");
    //        Assert.AreEqual(entity.Name, orm.Name, "Name");
    //        Assert.AreEqual(entity.DisplayOrder, orm.DisplayOrder, "DisplayOrder");
    //    }



    //}
}
