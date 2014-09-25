using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using NBehave.Spec.NUnit;
using Moq;

using Finances.Core.Interfaces;
using Finances.WinClient.DomainServices;
using Finances.Core.Entities;
using Finances.WinClient.ViewModels;
using Finances.WinClient.Mappers;

namespace Finances.UnitTests.Finances.WinClient.BankAccountServiceTests
{
    public class when_working_with_the_bank_account_service : Specification
    {
        protected IBankAccountService bankAccountService;
        protected Mock<IBankAccountRepository> bankAccountRepository;
        protected Mock<IBankAccountMapper> bankAccountMapper;

        protected override void Establish_context()
        {
            base.Establish_context();

            bankAccountRepository = new Mock<IBankAccountRepository>();
            bankAccountMapper = new Mock<IBankAccountMapper>();

            bankAccountService = new BankAccountService(bankAccountRepository.Object, bankAccountMapper.Object);

            bankAccountMapper.Setup(m => m.Map(It.IsAny<BankAccount>(), It.IsAny<IBankAccountItemViewModel>())).Callback<BankAccount, IBankAccountItemViewModel>((from, to) =>
            {
                to.BankAccountId = from.BankAccountId;
                to.AccountName = from.Name;
                to.Bank.BankId = from.Bank.BankId;
                to.Bank.Name = from.Bank.Name;
                to.Bank.LogoRaw = from.Bank.Logo;
            });
        }

    }

    public class and_deleting_a_valid_bank_view_model : when_working_with_the_bank_account_service
    {
        bool result;
        int testBankAccountId;
        int testRepBankAccountId;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBankAccountId = 123;

            bankAccountRepository.Setup(r => r.Delete(It.IsAny<int>())).Callback<int>(id => { testRepBankAccountId = id; }).Returns(true);

        }

        protected override void Because_of()
        {
            result = bankAccountService.Delete(testBankAccountId);
        }


        [Test]
        public void should_return_true()
        {
            result.ShouldEqual(true);
        }

        [Test]
        public void should_call_repository_delete()
        {
            bankAccountRepository.Verify(r => r.Delete(It.IsAny<int>()), Times.Once());
        }


        [Test]
        public void should_pass_id_to_repository()
        {
            testRepBankAccountId.ShouldEqual(testBankAccountId);
        }


    }


    public class and_reading_a_bank_item_view_model : when_working_with_the_bank_account_service
    {
        IBankAccountItemViewModel testBankAccountVM;
        BankAccount testBankAccount;
        Bank testBank;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBank = new Bank() { BankId = 1, Name = "HSBD" };
            testBankAccount = new BankAccount() { BankAccountId = 123, Name = "test", Bank = testBank };

            bankAccountRepository.Setup(r => r.Read(testBankAccount.BankAccountId)).Returns(testBankAccount);

        }

        protected override void Because_of()
        {
            testBankAccountVM = bankAccountService.CreateBankAccountViewModel();
            testBankAccountVM = bankAccountService.Read(testBankAccount.BankAccountId, testBankAccountVM);
        }


        [Test]
        public void should_call_repository_read()
        {
            bankAccountRepository.Verify(r => r.Read(It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void should_callback_with_valid_bank()
        {
            testBankAccount.ShouldNotBeNull();
        }

        [Test]
        public void should_read_valid_id()
        {
            testBankAccountVM.BankAccountId.ShouldEqual(testBankAccount.BankAccountId);
        }

        [Test]
        public void should_read_valid_properties()
        {
            testBankAccountVM.Bank.Name.ShouldEqual(testBankAccount.Bank.Name);
        }

    }


    public class and_reading_a_bank_account_view_model_list : when_working_with_the_bank_account_service
    {
        List<IBankAccountItemViewModel> testBankAccountVMList;
        List<BankAccount> testBankAccountList;
        Bank testBank;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBank = new Bank() { BankId = 1, Name = "HSBD" };

            testBankAccountList = new List<BankAccount>() {
                new BankAccount() { BankAccountId=1, Bank = testBank, Name = "test1" },
                new BankAccount() { BankAccountId=2, Bank = testBank, Name = "test2" },
                new BankAccount() { BankAccountId=3, Bank = testBank, Name = "test3" } };

            bankAccountRepository.Setup(r => r.ReadList()).Returns(testBankAccountList);

        }

        protected override void Because_of()
        {
            testBankAccountVMList = bankAccountService.ReadList();
        }


        [Test]
        public void should_call_repository_read_list()
        {
            bankAccountRepository.Verify(r => r.ReadList(), Times.Once());
        }

        [Test]
        public void should_callback_with_valid_bank_vm_list()
        {
            testBankAccountVMList.ShouldNotBeNull();
        }

        [Test]
        public void should_read_correct_number_of_banks()
        {
            testBankAccountVMList.Count.ShouldEqual(testBankAccountList.Count);
        }

        [Test]
        public void should_contain_valid_Bank_properties()
        {
            foreach (var item in testBankAccountVMList)
            {
                item.Bank.Name.ShouldEqual(testBank.Name);
            }
        }


    }

    public class and_reading_DataIdNameList : when_working_with_the_bank_account_service
    {
        List<DataIdName> input;
        List<DataIdName> result ;

        protected override void Establish_context()
        {
            base.Establish_context();


            input = new List<DataIdName>() {
                new DataIdName() { Id=1, Name = "test1" },
                new DataIdName() { Id=2, Name = "test2" },
                new DataIdName() { Id=3, Name = "test3" } };

            bankAccountRepository.Setup(r => r.ReadListDataIdName()).Returns(input);

        }

        protected override void Because_of()
        {
            result = bankAccountService.ReadListDataIdName();
        }


        [Test]
        public void should_call_repository_read_list()
        {
            bankAccountRepository.Verify(r => r.ReadListDataIdName(), Times.Once());
        }

        [Test]
        public void should_callback_with_valid_bank_vm_list()
        {
            result.ShouldNotBeNull();
        }

        [Test]
        public void should_read_correct_number_of_items()
        {
            result.Count.ShouldEqual(input.Count);
        }

        [Test]
        public void should_contain_valid_Bank_properties()
        {
            foreach (var item in result)
            {
                item.Name.ShouldEqual(input.Where(i => i.Id == item.Id).Single().Name);
            }
        }


    }

    public class and_adding_a_valid_bank_account_editor_view_model : when_working_with_the_bank_account_service
    {
        IBankAccountEditorViewModel testObject;
        BankAccount bankAccount;
        bool result;
        int bankAccountId = 123;


        protected override void Establish_context()
        {
            base.Establish_context();


            //bankAccount = new BankAccount();
            testObject = new BankAccountEditorViewModel(null,null)
                {
                    AccountName = "My qacount",
                    Bank = new BankItemViewModel() { BankId = 1, Name = "HSBV" }
                };


            bankAccountRepository.Setup(r => r.Add(It.IsAny<BankAccount>())).Returns(bankAccountId).Callback<BankAccount>(a => { bankAccount = a; });
            bankAccountMapper.Setup(m => m.Map(testObject, It.IsAny<BankAccount>())).Callback<IBankAccountEditorViewModel,BankAccount>((from, to) =>
                {
                    to.BankAccountId = from.BankAccountId;
                    to.Name = from.AccountName;
                    to.Bank.BankId = from.Bank.BankId;
                    to.Bank.Name = from.Bank.Name;
                }).Returns(bankAccount);
        

        }

        protected override void Because_of()
        {
            result = bankAccountService.Add(testObject);
        }

        [Test]
        public void should_return_true()
        {
            result.ShouldEqual(true);
        }

        [Test]
        public void should_give_valid_id()
        {
            testObject.BankAccountId.ShouldEqual(bankAccountId);
        }

        [Test]
        public void should_pass_details_to_repo()
        {
            testObject.AccountName.ShouldEqual(bankAccount.Name);
            testObject.Bank.BankId.ShouldEqual(bankAccount.Bank.BankId);
            testObject.Bank.Name.ShouldEqual(bankAccount.Bank.Name);
        }
    
    }



    public class and_updating_a_valid_bank_account_editor_view_model : when_working_with_the_bank_account_service
    {
        IBankAccountEditorViewModel testObject;
        BankAccount bankAccount;
        bool result;


        protected override void Establish_context()
        {
            base.Establish_context();


            testObject = new BankAccountEditorViewModel(null,null)
            {
                BankAccountId=123,
                AccountName = "My qacount",
                Bank = new BankItemViewModel() { BankId = 1, Name = "HSBV" }
            };


            bankAccountRepository.Setup(r => r.Update(It.IsAny<BankAccount>())).Returns(true).Callback<BankAccount>(a => { bankAccount = a; });
            bankAccountMapper.Setup(m => m.Map(testObject, It.IsAny<BankAccount>())).Callback<IBankAccountEditorViewModel, BankAccount>((from, to) =>
            {
                to.BankAccountId = from.BankAccountId;
                to.Name = from.AccountName;
                to.Bank.BankId = from.Bank.BankId;
                to.Bank.Name = from.Bank.Name;
            }).Returns<IBankAccountEditorViewModel, BankAccount>((f,t) => t);

        }

        protected override void Because_of()
        {
            result = bankAccountService.Update(testObject);
        }

        [Test]
        public void should_return_true()
        {
            result.ShouldEqual(true);
        }

        [Test]
        public void should_give_valid_id()
        {
            testObject.BankAccountId.ShouldEqual(bankAccount.BankAccountId);
        }

        [Test]
        public void should_pass_details_to_repo()
        {
            testObject.AccountName.ShouldEqual(bankAccount.Name);
            testObject.Bank.BankId.ShouldEqual(bankAccount.Bank.BankId);
            testObject.Bank.Name.ShouldEqual(bankAccount.Bank.Name);
        }

    }


    public class and_reading_a_valid_bank_account_editor_view_model : when_working_with_the_bank_account_service
    {
        IBankAccountEditorViewModel testObject;
        BankAccount bankAccount;
        bool result;


        protected override void Establish_context()
        {
            base.Establish_context();


            testObject = new BankAccountEditorViewModel(null, null) { Bank = new BankItemViewModel() };

            bankAccount = new BankAccount()
            {
                BankAccountId = 123,
                Name = "My qacount",
                Bank = new Bank() { BankId = 1, Name = "HSBV" }
            };


            bankAccountRepository.Setup(r => r.Read(bankAccount.BankAccountId)).Returns(bankAccount);
            bankAccountMapper.Setup(m => m.Map(bankAccount, testObject)).Callback<BankAccount, IBankAccountEditorViewModel>((from, to) =>
            {
                to.BankAccountId = from.BankAccountId;
                to.AccountName = from.Name;
                to.Bank.BankId = from.Bank.BankId;
                to.Bank.Name = from.Bank.Name;
            }).Returns<BankAccount, IBankAccountEditorViewModel>((f, t) => t);

        }

        protected override void Because_of()
        {
            testObject = bankAccountService.Read(bankAccount.BankAccountId, testObject);
        }


        [Test]
        public void should_give_valid_id()
        {
            testObject.BankAccountId.ShouldEqual(bankAccount.BankAccountId);
        }

        [Test]
        public void should_get_details_from_repo()
        {
            testObject.AccountName.ShouldEqual(bankAccount.Name);
            testObject.Bank.BankId.ShouldEqual(bankAccount.Bank.BankId);
            testObject.Bank.Name.ShouldEqual(bankAccount.Bank.Name);
        }

    }


}
