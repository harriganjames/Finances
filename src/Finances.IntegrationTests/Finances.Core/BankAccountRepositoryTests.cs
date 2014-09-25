
using System.Configuration;
using Finances.Core.Entities;
using Finances.InegrationTests;
using NUnit.Framework;
using NBehave.Spec.NUnit;
using Finances.Core.Interfaces;
using FluentNHibernate;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Finances.Persistence.FNH.Mappings;
using NHibernate.Tool.hbm2ddl;
using Finances.Persistence.FNH;
using System;


namespace Finances.IntegrationTests.Finances.Core.BankAccountRepositoryTests
{
    public class when_using_the_bank_account_repository : Specification
    {
        protected IBankRepository _bankRepository;
        protected IBankAccountRepository _bankAccountRepository;


        protected override void Establish_context()
        {
            base.Establish_context();

            string conn = "Server=localhost; Database=FinanceINT; Integrated Security=true";// ConfigurationManager.AppSettings["localDB"];

            //int i = conn.Length;

            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2008.ConnectionString(c => c.Is(conn))
                    .ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<BankMap>()
                    .ExportTo(@"C:\Temp"))
                .BuildSessionFactory();

            //.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true,true))

            _bankRepository = new BankRepository(sessionFactory);
            _bankAccountRepository = new BankAccountRepository(sessionFactory);

        }
    }

    public class and_attempting_to_add_and_read_from_the_database : when_using_the_bank_account_repository
    {
        BankAccount _result;
        BankAccount _expected;

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

            //Bank b = new Bank() { Name = Guid.NewGuid().ToString(), Logo = Guid.NewGuid().ToByteArray() };
            Bank b = new Bank() { Name = Guid.NewGuid().ToString() };
            _expected = new BankAccount() { Name = Guid.NewGuid().ToString(), Bank = b, OpenedDate=DateTime.Now.Date, AccountOwner="me" };
        }

        protected override void Because_of()
        {
            _bankRepository.Add(_expected.Bank);
            int id = _bankAccountRepository.Add(_expected);
            this._result = _bankAccountRepository.Read(id);
        }

        [Test]
        public void then_the_values_should_be_equal()
        {
            _result.BankAccountId.ShouldEqual(_expected.BankAccountId);
            _result.Name.ShouldEqual(_expected.Name);
            _result.Bank.BankId.ShouldEqual(_expected.Bank.BankId);
            _result.Bank.Name.ShouldEqual(_expected.Bank.Name);
        }
    }
}
