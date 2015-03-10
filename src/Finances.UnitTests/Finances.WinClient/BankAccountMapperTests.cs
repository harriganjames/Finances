using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using NBehave.Spec.NUnit;
using Moq;
using Finances.Core.Entities;
using Finances.WinClient.ViewModels;
//using Finances.WinClient.Mappers;


//in progress Bank Map(BankEditorViewModel from, Bank to);
//done BankAccountItemViewModel Map(BankAccount from, BankAccountItemViewModel to);
//BankEditorViewModel Map(Bank from, BankEditorViewModel to);

namespace Finances.UnitTests.Finances.WinClient.BankAccountMapperTests
{
    //public class when_working_with_the_bank_account_mapper : Specification
    //{
    //    protected IBankAccountMapper bankAccountMapper;

    //    //protected IBankMapper bankMapper;

    //    protected override void Establish_context()
    //    {
    //        base.Establish_context();

    //        //bankMapper = new BankMapper();

    //        bankAccountMapper = new BankAccountMapper();
    //    }


    //}


    //public class and_mapping_a_bank_account_to_a_bank_account_item_VM : when_working_with_the_bank_account_mapper
    //{
    //    BankAccount from;
    //    BankAccountItemViewModel to;

    //    protected override void Establish_context()
    //    {
    //        base.Establish_context();

    //        // should these be mocked?

    //        to = new BankAccountItemViewModel();
    //        to.Bank = new BankItemViewModel();
    //        from = new BankAccount() { BankAccountId = 123, Name = "test", Bank = new Bank() { BankId = 1, Name = "HSBC" } };
    //    }

    //    protected override void Because_of()
    //    {
    //        bankAccountMapper.Map(from, to);
    //    }


    //    [Test]
    //    public void should_return_bank_account_with_valid_id()
    //    {
    //        to.BankAccountId.ShouldEqual(from.BankAccountId);
    //    }

    //    [Test]
    //    public void should_return_bank_account_with_valid_name()
    //    {
    //        to.AccountName.ShouldEqual(from.Name);
    //    }

    //    [Test]
    //    public void should_return_bank_account_with_valid_bank_name()
    //    {
    //        to.Bank.Name.ShouldEqual(from.Bank.Name);
    //        //to.Bank.ShouldNotBeNull();
    //        //from.Bank.ShouldNotBeNull();
    //    }

    //}


    //public class and_mapping_a_bank_account_editor_VM_to_a_bank_account : when_working_with_the_bank_account_mapper
    //{
    //    BankAccount to;
    //    BankAccountEditorViewModel from;

    //    protected override void Establish_context()
    //    {
    //        base.Establish_context();


    //        BankItemViewModel bank = new BankItemViewModel() { BankId = 1, Name = "HSBC" };
    //        from = new BankAccountEditorViewModel(null) { BankAccountId = 123, AccountName = "test", Bank = bank };
    //        to = new BankAccount();// { Bank = new Bank() };
    //    }

    //    protected override void Because_of()
    //    {
    //        bankAccountMapper.Map(from, to);
    //    }


    //    [Test]
    //    public void should_return_bank_account_with_valid_id()
    //    {
    //        to.BankAccountId.ShouldEqual(from.BankAccountId);
    //    }

    //    [Test]
    //    public void should_return_bank_account_with_valid_name()
    //    {
    //        to.Name.ShouldEqual(from.AccountName);
    //    }

    //    [Test]
    //    public void bank_should_not_be_null()
    //    {
    //        to.Bank.ShouldNotBeNull();
    //    }
    //    [Test]
    //    public void vm_bank_should_not_be_null()
    //    {
    //        from.Bank.ShouldNotBeNull();
    //    }

    //    [Test]
    //    public void should_return_bank_account_with_valid_bank()
    //    {
    //        to.Bank.BankId.ShouldEqual(from.Bank.BankId);
    //    }

    //}




    //public class and_mapping_a_bank_account_to_a_bank_editor_VM : when_working_with_the_bank_account_mapper
    //{
    //    BankAccount from;
    //    BankAccountEditorViewModel to;

    //    protected override void Establish_context()
    //    {
    //        base.Establish_context();


    //        BankItemViewModel bank = new BankItemViewModel();
    //        to = new BankAccountEditorViewModel(null) { Bank = bank };
    //        from = new BankAccount() { BankAccountId = 123, Name = "My Account", Bank = new Bank() { BankId=1, Name="HSVD" } };
    //    }

    //    protected override void Because_of()
    //    {
    //        bankAccountMapper.Map(from, to);
    //    }


    //    [Test]
    //    public void should_return_bank_account_with_valid_id()
    //    {
    //        to.BankAccountId.ShouldEqual(from.BankAccountId);
    //    }

    //    [Test]
    //    public void should_return_bank_account_with_valid_name()
    //    {
    //        to.AccountName.ShouldEqual(from.Name);
    //    }

    //    [Test]
    //    public void bank_should_not_be_null()
    //    {
    //        to.Bank.ShouldNotBeNull();
    //    }
    //    [Test]
    //    public void vm_bank_should_not_be_null()
    //    {
    //        from.Bank.ShouldNotBeNull();
    //    }

    //    [Test]
    //    public void should_return_bank_account_with_valid_bank()
    //    {
    //        to.Bank.BankId.ShouldEqual(from.Bank.BankId);
    //    }

    //}


}
