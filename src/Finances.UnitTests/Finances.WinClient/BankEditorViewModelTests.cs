using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using NBehave.Spec.NUnit;
using Moq;
using Finances.WinClient.ViewModels;
using Finances.WinClient.DomainServices;
using Finances.Core.Wpf;
using Finances.Core.Entities;
using Finances.Core.Interfaces;


namespace Finances.UnitTests.Finances.WinClient.BankEditorViewModelTests
{
    public class when_working_with_the_bank_editor_view_model : Specification
    {
        protected IBankEditorViewModel bankEditorViewModel;

        //protected Mock<IBankService> bankService;
        protected Mock<IBankRepository> bankRepository;
        protected Mock<IDialogService> dialogService;


        protected override void Establish_context()
        {
            base.Establish_context();

            bankRepository = new Mock<IBankRepository>();
            dialogService = new Mock<IDialogService>();

            bankEditorViewModel = new BankEditorViewModel(bankRepository.Object, dialogService.Object);

        }
    }

    public class and_inputting_an_existing_bank : when_working_with_the_bank_editor_view_model
    {

        protected override void Establish_context()
        {
            base.Establish_context();
            List<DataIdName> dataList;

            dataList = new List<DataIdName> {
                    new DataIdName() { Id=1, Name="abc" }
                };

            base.bankRepository.Setup(bs => bs.ReadListDataIdName()).Returns(dataList);

        }

        protected override void Because_of()
        {
            base.bankEditorViewModel.InitializeForAddEdit(true);
            base.bankEditorViewModel.BankId = 2;
            base.bankEditorViewModel.Name = "abc";
        }

        [Test]
        public void should_not_be_valid()
        {
            bankEditorViewModel.IsValid.ShouldBeFalse();
        }

        [Test]
        public void validation_error_should_contain_already_exists()
        {
            bankEditorViewModel.Errors.FirstOrDefault().ShouldContain("already exists");
        }


    }

}
