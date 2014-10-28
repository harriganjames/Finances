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



namespace Finances.UnitTests.MOVEDTOMS.Finances.WinClient.BankAccountEditorViewModelTests
{
    public class when_working_with_the_bank_account_editor_view_model : Specification
    {
        protected IBankAccountEditorViewModel bankAccountEditorViewModel;

        protected Mock<IBankAccountService> bankAccountService;
        //protected Mock<IDialogService> dialogService;

        protected override void Establish_context()
        {
            base.Establish_context();

            bankAccountService = new Mock<IBankAccountService>();
            //dialogService = new Mock<IDialogService>();

            bankAccountEditorViewModel = new BankAccountEditorViewModel(bankAccountService.Object);
        
        }
    }


    public class and_inputting_an_existing_bank : when_working_with_the_bank_account_editor_view_model
    {

        protected override void Establish_context()
        {
            base.Establish_context();
            List<DataIdName> dataList;

            dataList = new List<DataIdName> {
                    new DataIdName() { Id=1, Name="abc (xyz)" }
                };

            base.bankAccountService.Setup(bs => bs.ReadListDataIdName()).Returns(dataList);

            List<IBankItemViewModel> banks = new List<IBankItemViewModel>();

            base.bankAccountService.Setup(bs => bs.ReadBankList()).Returns(banks);

        }

        protected override void Because_of()
        {
            base.bankAccountEditorViewModel.InitializeForAddEdit(true);
            base.bankAccountEditorViewModel.BankAccountId = 2;
            base.bankAccountEditorViewModel.AccountName = "abc";
            base.bankAccountEditorViewModel.Bank = new BankItemViewModel() { Name = "xyz" };
        }

        [Test]
        public void should_not_be_valid()
        {
            bankAccountEditorViewModel.IsValid.ShouldBeFalse();
        }

        [Test]
        public void validation_error_should_contain_already_exists()
        {
            bankAccountEditorViewModel.Errors.FirstOrDefault().ShouldContain("already exists");
        }


    }




}
