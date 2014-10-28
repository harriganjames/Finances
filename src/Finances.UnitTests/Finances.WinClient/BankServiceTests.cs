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
//using Finances.WinClient.Mappers;

namespace Finances.UnitTests.Finances.WinClient.BankServiceTests
{
    public class when_working_with_the_bank_service : Specification
    {
        protected IBankService bankService;
        protected Mock<IBankRepository> bankRepository;
        //protected Mock<IBankMapper> bankMapper;
        //protected Mock<IBankViewModelFactory> bankViewModelFactory;

        protected override void Establish_context()
        {
            base.Establish_context();

            bankRepository = new Mock<IBankRepository>();
            //bankMapper = new Mock<IBankMapper>();
            //bankViewModelFactory = new Mock<IBankViewModelFactory>();

            bankService = new BankService(bankRepository.Object);
        }


    }

    public class and_adding_a_valid_bank_editor_view_model : when_working_with_the_bank_service
    {
        bool result;
        BankEditorViewModel testBankVM;

        protected override void Establish_context()
        {
            base.Establish_context();

            bankRepository.Setup(b => b.Add(It.IsAny<Bank>())).Returns(123);
            //bankMapper.Setup(m=>m.Map(testBankVM,It.IsAny<Bank>()));

            testBankVM = new BankEditorViewModel(null,null);
        
        }
        
        protected override void Because_of()
        {
            result = bankService.Add(testBankVM);
        }


        [Test]
        public void should_return_vm_with_valid_id()
        {
            result.ShouldEqual(true);
            testBankVM.BankId.ShouldEqual(123);
        }

        [Test]
        public void should_call_repository_add()
        {
            bankRepository.Verify(r => r.Add(It.IsAny<Bank>()), Times.Once());
        }
    }

    public class and_updating_a_valid_bank_editor_view_model : when_working_with_the_bank_service
    {
        bool result;
        BankEditorViewModel testBankVM;
        Bank testBank;

        protected override void Establish_context()
        {
            base.Establish_context();

            bankRepository.Setup(r => r.Update(It.IsAny<Bank>())).Callback<Bank>(b => { testBank = b; }).Returns(true);

            //bankMapper.Setup(m => m.Map(It.IsAny<IBankEditorViewModel>(), It.IsAny<Bank>())).Callback<IBankEditorViewModel, Bank>((vm, b) =>
            //    {
            //        b.BankId = vm.BankId;
            //        b.Name = vm.Name;
            //    });

            testBankVM = new BankEditorViewModel(null,null) { BankId = 123, Name = "test" };

        }

        protected override void Because_of()
        {
            result = bankService.Update(testBankVM);
        }


        [Test]
        public void should_return_true()
        {
            result.ShouldEqual(true);
        }

        [Test]
        public void should_call_repository_update()
        {
            bankRepository.Verify(r => r.Update(It.IsAny<Bank>()), Times.Once());
        }

        [Test]
        public void should_callback_with_valid_bank()
        {
            testBank.ShouldNotBeNull();
        }

        [Test]
        public void should_pass_id_to_repository()
        {
            testBankVM.BankId.ShouldEqual(testBank.BankId);
        }

        [Test]
        public void should_pass_name_to_repository()
        {
            testBankVM.Name.ShouldEqual(testBank.Name);
        }

    }

    public class and_deleting_a_valid_bank_view_model : when_working_with_the_bank_service
    {
        bool result;
        int testBankId;
        int testRepBankId;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBankId = 123;

            bankRepository.Setup(r => r.Delete(It.IsAny<Bank>())).Callback<Bank>(b => { testRepBankId = b.BankId; }).Returns(true);

        }

        protected override void Because_of()
        {
            result = bankService.Delete(new BankItemViewModel() { BankId = testBankId });
        }


        [Test]
        public void should_return_true()
        {
            result.ShouldEqual(true);
        }

        [Test]
        public void should_call_repository_delete()
        {
            bankRepository.Verify(r => r.Delete(It.IsAny<Bank>()), Times.Once());
        }


        [Test]
        public void should_pass_id_to_repository()
        {
            testRepBankId.ShouldEqual(testBankId);
        }


    }

    public class and_reading_a_bank_item_view_model : when_working_with_the_bank_service
    {
        IBankItemViewModel testBankVM;
        Bank testBank;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBank = new Bank() { BankId = 123, Name = "test" };

            bankRepository.Setup(r => r.Read(testBank.BankId)).Returns(testBank);
            //bankMapper.Setup(m => m.Map(It.IsAny<Bank>(), It.IsAny<IBankItemViewModel>())).Callback<Bank, IBankItemViewModel>((from, to) =>
            //{
            //    to.BankId = from.BankId;
            //    to.Name = from.Name;
            //});
            //bankViewModelFactory.Setup(f => f.CreateBankViewModel()).Returns(new BankViewModel(null));

        }

        protected override void Because_of()
        {
            testBankVM = bankService.CreateBankItemViewModel();
            bankService.Read(testBank.BankId, testBankVM);
        }


        [Test]
        public void should_call_repository_read()
        {
            bankRepository.Verify(r => r.Read(It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void should_callback_with_valid_bank()
        {
            testBank.ShouldNotBeNull();
        }

        [Test]
        public void should_read_valid_id()
        {
            testBankVM.BankId.ShouldEqual(testBank.BankId);
        }
    }

    public class and_reading_a_bank_view_model_list : when_working_with_the_bank_service
    {
        List<IBankItemViewModel> testBankVMList;
        List<Bank> testBankList;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBankList = new List<Bank>() {
                new Bank() { BankId = 1, Name = "test1" },
                new Bank() { BankId = 2, Name = "test2" },
                new Bank() { BankId = 3, Name = "test3" } };

            bankRepository.Setup(r => r.ReadList()).Returns(testBankList);
            //bankMapper.Setup(m => m.Map(It.IsAny<Bank>(), It.IsAny<IBankItemViewModel>())).Callback<Bank, IBankItemViewModel>((from, to) =>
            //{
            //    to.BankId = from.BankId;
            //    to.Name = from.Name;
            //});
            //bankViewModelFactory.Setup(f => f.CreateBankViewModel()).Returns(new BankViewModel(null));

        }

        protected override void Because_of()
        {
            testBankVMList = bankService.ReadList();
        }


        [Test]
        public void should_call_repository_read_list()
        {
            bankRepository.Verify(r => r.ReadList(), Times.Once());
        }

        [Test]
        public void should_callback_with_valid_bank_vm_list()
        {
            testBankVMList.ShouldNotBeNull();
        }

        [Test]
        public void should_read_correct_number_of_banks()
        {
            testBankVMList.Count.ShouldEqual(testBankList.Count);
        }


    }


}
