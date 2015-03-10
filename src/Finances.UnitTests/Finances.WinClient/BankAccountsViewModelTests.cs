//using System;
//using System.Collections.Generic;
//using System.Linq;

//using System.Collections.ObjectModel;
//using System.Threading.Tasks;
//using Finances.WinClient.DomainServices;
//using Finances.WinClient.ViewModels;

//using Moq;
//using NUnit.Framework;
//using NBehave.Spec.NUnit;

//namespace Finances.UnitTests.Finances.WinClient.BankAccountsViewModelTests
//{
//    public class when_working_with_the_bank_accounts_view_model : Specification
//    {
//        protected IBankAccountsViewModel bankAccountsViewModel;

//        protected Mock<IBankAccountService> bankService;

//        protected override void Establish_context()
//        {
//            base.Establish_context();

//            bankService = new Mock<IBankAccountService>();

//            bankAccountsViewModel = new BankAccountsViewModel(bankService.Object,null,null);

//        }

//    }

//    public class and_loading_a_list_of_bank_account_view_models : when_working_with_the_bank_accounts_view_model
//    {
//        List<BankAccountItemViewModel> testBankAccountVMList;
//        ObservableCollection<BankAccountItemViewModel> testResult;

//        public void MainSetup()
//        {
//            base.MainSetup();
//        }

//        public void MainTeardown()
//        {
//            base.MainTeardown();
//        }


//        protected override void Establish_context()
//        {
//            base.Establish_context();


//            testBankAccountVMList = new List<BankAccountItemViewModel>() 
//            {
//                new BankAccountItemViewModel() { BankAccountId=1, AccountName="One", Bank=new BankItemViewModel() { BankId=1, Name="HSBC" } },
//                new BankAccountItemViewModel() { BankAccountId=2, AccountName="Two", Bank=new BankItemViewModel() { BankId=1, Name="HSBC" } },
//                new BankAccountItemViewModel() { BankAccountId=3, AccountName="Three", Bank=new BankItemViewModel() { BankId=1, Name="HSBC" } },
//            };

//            bankService.Setup(s => s.ReadList()).Returns(testBankAccountVMList);

//        }

//        protected override void Because_of()
//        {
//            bankAccountsViewModel.ReloadCommand.Execute(null);

//            Task t = new Task(() =>
//                {
//                    for (int i = 0; i < 50; i++)
//                    {
//                        if (bankAccountsViewModel.IsBusy)
//                            Task.Delay(100).Wait();

//                    }
//                });
//            t.Start();
//            Task.WaitAll(t);

//            testResult = bankAccountsViewModel.BankAccounts;
//        }

//        [Test]
//        public void should_not_be_busy()
//        {
//            bankAccountsViewModel.IsBusy.ShouldEqual(false);
//        }


//        [Test]
//        public void should_return_correct_quantity()
//        {
//            testResult.Count().ShouldEqual(testBankAccountVMList.Count());
//        }
//    }
//}
