using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Finances.WinClient.DomainServices;
using Finances.Core.Interfaces;
using Finances.InegrationTests;
using Finances.Persistence.FNH;
using Finances.Persistence.FNH.Mappings;
using FluentNHibernate.Cfg;
using NHibernate;
using Finances.IntegrationTests.CastleInstallers;
using Finances.WinClient.ViewModels;
using NUnit.Framework;
using NBehave.Spec.NUnit;

namespace Finances.IntegrationTests.Finances.Core
{
    //public class when_using_the_banks_view_model : Specification
    //{
    //    protected IBankService _bankService;
    //    protected IBanksViewModel banksViewModel;

    //    protected override void Establish_context()
    //    {
    //        base.Establish_context();

    //        var container = new WindsorContainer();

    //        container.Install(new NHibernateSessionFactoryInstaller(),
    //                        new BankRepositoryInstaller(),
    //                        new BankServiceInstaller()
    //                        );

    //        this.banksViewModel = container.Resolve<IBanksViewModel>();
    //    }

    //}

    //public class and_adding_and_reading_a_bank_editor_view_model : when_using_the_banks_view_model
    //{
    //    IBankEditorViewModel _result;
    //    IBankEditorViewModel _expected;

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

    //        _expected = new BankEditorViewModel(null,null) { Name = Guid.NewGuid().ToString(), LogoRaw = Guid.NewGuid().ToByteArray() };

    //        _result = new BankEditorViewModel(null, null);
    //    }

    //    protected override void Because_of()
    //    {
    //        _bankService.Add(_expected);
    //        _bankService.Read(_expected.BankId,this._result);
    //    }

    //    [Test]
    //    public void then_the_values_should_be_equal()
    //    {
    //        _result.BankId.ShouldEqual(_expected.BankId);
    //        _result.Name.ShouldEqual(_expected.Name);
    //        _result.LogoRaw.ShouldEqual(_expected.LogoRaw);
    //    }
    //}

}
