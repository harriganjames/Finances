using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using NBehave.Spec.NUnit;
using Moq;
//using Finances.Core.Entities;
//using Finances.WinClient.ViewModels;
//using Finances.WinClient.Mappers;
using Finances.WinClient.DomainServices;
using Finances.WinClient.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Finances.UnitTests.Finances.WinClient.BankViewModelTests
{
    //public class when_working_with_the_banks_view_model : Specification
    //{
    //    protected IBanksViewModel banksViewModel;

    //    protected Mock<IBankService> bankService;

    //    protected override void Establish_context()
    //    {
    //        base.Establish_context();

    //        bankService = new Mock<IBankService>();

    //        banksViewModel = new BanksViewModel(bankService.Object,null,null);

    //    }


    //}

    //public class and_loading_a_list_of_bank_view_models : when_working_with_the_banks_view_model
    //{
    //    List<BankItemViewModel> testBankVMList;
    //    ObservableCollection<BankItemViewModel> testResult;

    //    public void MainSetup()
    //    {
    //        base.MainSetup();
    //    }

    //    public void MainTeardown()
    //    {
    //        base.MainTeardown();
    //    }


    //    protected override void Establish_context()
    //    {
    //        base.Establish_context();


    //        testBankVMList = new List<BankItemViewModel>() 
    //        {
    //            new BankItemViewModel() { BankId=1, Name="One" },
    //            new BankItemViewModel() { BankId=2, Name="Two" },
    //            new BankItemViewModel() { BankId=3, Name="Three" },
    //        };

    //        bankService.Setup(s => s.ReadList()).Returns(testBankVMList);

    //    }

    //    protected override void Because_of()
    //    {
    //        banksViewModel.ReloadCommand.Execute(null);

    //        Task t = new Task(() =>
    //            {
    //                for (int i = 0; i < 50; i++)
    //                {
    //                    if(banksViewModel.IsBusy)
    //                        Task.Delay(100).Wait();
                        
    //                }
    //            });
    //        t.Start();
    //        Task.WaitAll(t);

    //        testResult = banksViewModel.Banks;
    //    }

    //    [Test]
    //    public void should_not_be_busy()
    //    {
    //        banksViewModel.IsBusy.ShouldEqual(false);
    //    }


    //    [Test]
    //    public void should_return_correct_quantity()
    //    {
    //        testResult.Count().ShouldEqual(testBankVMList.Count());
    //    }


    //}



}