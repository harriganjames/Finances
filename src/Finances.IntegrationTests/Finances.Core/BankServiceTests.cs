//using System;
//using System.Collections.Generic;
//using Castle.MicroKernel.Registration;
//using Castle.Windsor;
//using Finances.WinClient.DomainServices;
//using Finances.Core.Interfaces;
//using Finances.InegrationTests;
//using Finances.Persistence.FNH;
//using Finances.Persistence.FNH.Mappings;
//using FluentNHibernate.Cfg;
//using NHibernate;
//using Finances.IntegrationTests.CastleInstallers;
//using Finances.WinClient.ViewModels;
//using NUnit.Framework;
//using NBehave.Spec.NUnit;
//using Finances.Core.Wpf;

//namespace Finances.IntegrationTests.Finances.Core.BankServiceTests
//{
//    public class when_using_the_bank_service : Specification
//    {
//        protected IBankService _bankService;


//        protected override void Establish_context()
//        {
//            base.Establish_context();

//            var container = new WindsorContainer();
            
//            container.Install(new NHibernateSessionFactoryInstaller(),
//                            new BankRepositoryInstaller(),
//                            new BankServiceInstaller()
//                            );

//            container.Register(Component.For<IDialogService>().UsingFactoryMethod(() => null as IDialogService));

//            _bankService = container.Resolve<IBankService>();
//        }

//    }

//    public class and_attempting_to_add_and_read_a_value_from_the_service : when_using_the_bank_service
//    {
//        BankEditorViewModel _result;
//        BankEditorViewModel _expected;

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

//            _expected = new BankEditorViewModel(null,null) { Name = Guid.NewGuid().ToString(), LogoRaw = Guid.NewGuid().ToByteArray() };

//            _result = new BankEditorViewModel(null, null);
//        }

//        protected override void Because_of()
//        {
//            _bankService.Add(_expected);
//            _bankService.Read(_expected.BankId,this._result);
//        }

//        [Test]
//        public void then_the_values_should_be_equal()
//        {
//            _result.BankId.ShouldEqual(_expected.BankId);
//            _result.Name.ShouldEqual(_expected.Name);
//            _result.LogoRaw.ShouldEqual(_expected.LogoRaw);
//        }
//    }

//}
