using System;
using Finances.Core.Entities;
//using Finances.WinClient.Mappers;
using Finances.WinClient.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Finances.UnitTests.MS
{
    //public abstract class BankAccountMapperTests
    //{
    //    protected IBankAccountMapper bankAccountMapper;

    //    //protected IBankMapper bankMapper;

    //    public virtual void Initialize()
    //    {

    //        //bankMapper = new BankMapper();

    //        bankAccountMapper = new BankAccountMapper();

    //    }

    //}


    //[TestClass]
    //public class when_mapping_a_bank_account_editor_VM_to_a_bank_account : BankAccountMapperTests
    //{
    //    BankAccount to;
    //    IBankAccountEditorViewModel from;


    //    [TestInitialize]
    //    public override void Initialize()
    //    {
    //        base.Initialize();


    //        BankItemViewModel bank = new BankItemViewModel() { BankId = 1, Name = "HSBC" };
    //        from = new BankAccountEditorViewModel(null) { BankAccountId = 123, AccountName = "test", Bank = bank };
    //        to = new BankAccount();// { Bank = new Bank() };


    //        bankAccountMapper.Map(from, to);
    //    }



    //    [TestMethod]
    //    public void should_return_bank_account_with_valid_id()
    //    {
    //        Assert.AreEqual(to.BankAccountId, from.BankAccountId);
    //    }

    //    [TestMethod]
    //    public void should_return_bank_account_with_valid_name()
    //    {
    //        Assert.AreEqual(to.Name, from.AccountName);
    //    }

    //    [TestMethod]
    //    public void bank_should_not_be_null()
    //    {
    //        Assert.IsNotNull(to.Bank);
    //    }
    //    [TestMethod]
    //    public void vm_bank_should_not_be_null()
    //    {
    //        Assert.IsNotNull(from.Bank);
    //    }

    //    [TestMethod]
    //    public void should_return_bank_account_with_valid_bank()
    //    {
    //        Assert.AreEqual(to.Bank.BankId, from.Bank.BankId);
    //    }

    //}

}
