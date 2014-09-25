using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using NBehave.Spec.NUnit;
using Moq;
using Finances.Core.Entities;
using Finances.WinClient.ViewModels;
using Finances.WinClient.Mappers;

namespace Finances.UnitTests.Finances.WinClient.BankMapperTests
{
    public class when_working_with_the_bank_mapper : Specification
    {
        protected IBankMapper bankMapper;

        protected override void Establish_context()
        {
            base.Establish_context();

            bankMapper = new BankMapper();
        }


    }

    public class and_mapping_a_bank_editor_VM_to_a_bank : when_working_with_the_bank_mapper
    {
        Bank result;
        IBankEditorViewModel testBankVM;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBankVM = new BankEditorViewModel(null,null) { BankId=123, Name="test", LogoRaw=new byte[] { 1, 2, 3 } };
            result = new Bank();
        }
        
        protected override void Because_of()
        {
            bankMapper.Map(testBankVM,result);
        }


        [Test]
        public void should_return_vm_with_valid_id()
        {
            result.BankId.ShouldEqual(testBankVM.BankId);
        }

        [Test]
        public void should_return_vm_with_valid_name()
        {
            result.Name.ShouldEqual(testBankVM.Name);
        }

        [Test]
        public void should_return_vm_with_valid_logo()
        {
            result.Logo.ShouldEqual(testBankVM.LogoRaw);
        }

    }


    public class and_mapping_a_bank_to_a_bank_editor_VM : when_working_with_the_bank_mapper
    {
        Bank result;
        IBankEditorViewModel testBankVM;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBankVM = new BankEditorViewModel(null,null);
            result = new Bank() { BankId = 123, Name = "test", Logo = new byte[] { 1, 2, 3 } };
        }

        protected override void Because_of()
        {
            bankMapper.Map(result, testBankVM);
        }


        [Test]
        public void should_return_bank_with_valid_id()
        {
            result.BankId.ShouldEqual(testBankVM.BankId);
        }

        [Test]
        public void should_return_bank_with_valid_name()
        {
            result.Name.ShouldEqual(testBankVM.Name);
        }

        [Test]
        public void should_return_bank_with_valid_logo()
        {
            result.Logo.ShouldEqual(testBankVM.LogoRaw);
        }

    }

    public class and_mapping_a_bank_to_a_bank_item_VM : when_working_with_the_bank_mapper
    {
        Bank result;
        IBankItemViewModel testBankVM;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBankVM = new BankItemViewModel();
            result = new Bank() { BankId = 123, Name = "test", Logo = new byte[] { 1, 2, 3 } };
        }

        protected override void Because_of()
        {
            bankMapper.Map(result, testBankVM);
        }


        [Test]
        public void should_return_bank_with_valid_id()
        {
            result.BankId.ShouldEqual(testBankVM.BankId);
        }

        [Test]
        public void should_return_bank_with_valid_name()
        {
            result.Name.ShouldEqual(testBankVM.Name);
        }

        [Test]
        public void should_return_bank_with_valid_logo()
        {
            result.Logo.ShouldEqual(testBankVM.LogoRaw);
        }

    }

}
