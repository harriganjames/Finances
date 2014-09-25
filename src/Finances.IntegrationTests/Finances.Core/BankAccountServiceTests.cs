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
using Finances.Core.Wpf;
using Finances.WinClient.Mappers;

namespace Finances.IntegrationTests.Finances.Core.BankAccountServiceTests
{
    public class when_using_the_bank_account_service : Specification
    {
        protected IBankService _bankService;
        protected IBankAccountService _bankAccountService;


        protected override void Establish_context()
        {
            base.Establish_context();

            var container = new WindsorContainer();

            container.Install(new NHibernateSessionFactoryInstaller(),
                            new BankRepositoryInstaller(),
                            new BankServiceInstaller()
                            );

            container.Register(Component.For<IDialogService>().UsingFactoryMethod(() => null as IDialogService));

            _bankService = container.Resolve<IBankService>();

            var sessionFactory = container.Resolve<ISessionFactory>();

            _bankAccountService = new BankAccountService(new BankAccountRepository(sessionFactory), new BankAccountMapper(new BankMapper()));

        }

    }

    public class and_attempting_to_read_a_value_from_the_service : when_using_the_bank_account_service
    {
        IBankAccountItemViewModel _result;
        //IBankEditorViewModel _expected;

        public void MainSetup()
        {
            base.MainSetup();
        }

        public void MainTeardown()
        {
            base.MainTeardown();
        }


        protected override void Establish_context()
        {
            base.Establish_context();

        }

        protected override void Because_of()
        {
            //_bankService.Add(_expected);
            _result = new BankAccountItemViewModel() { Bank = new BankItemViewModel() };
            this._result = _bankAccountService.Read(1, _result);
        }

        [Test]
        public void then_the_values_should_not_be_null()
        {
            _result.BankAccountId.ShouldNotBeNull();
            _result.Bank.BankId.ShouldNotBeNull();
            _result.Bank.Name.ShouldNotBeNull();
        }
    }


    public class and_attempting_to_read_a_list_of_values_from_the_service : when_using_the_bank_account_service
    {
        List<IBankAccountItemViewModel> _result;
        //IBankEditorViewModel _expected;

        public void MainSetup()
        {
            base.MainSetup();
        }

        public void MainTeardown()
        {
            base.MainTeardown();
        }


        protected override void Establish_context()
        {
            base.Establish_context();

        }

        protected override void Because_of()
        {
            this._result = _bankAccountService.ReadList();
        }

        [Test]
        public void then_the_values_should_not_be_null()
        {
            foreach (var item in _result)
            {
                item.BankAccountId.ShouldNotBeNull();
                item.Bank.BankId.ShouldNotBeNull();
                item.Bank.Name.ShouldNotBeNull();
            }
        }
    }

}
