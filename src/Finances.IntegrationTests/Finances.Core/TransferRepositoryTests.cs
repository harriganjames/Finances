
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


namespace Finances.IntegrationTests.Finances.Core.TransferRepositoryTests
{
    public class when_using_the_bank_account_repository : Specification
    {
        protected IBankRepository _bankRepository;
        protected ITransferRepository _TransferRepository;


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
            _TransferRepository = new TransferRepository(sessionFactory);

        }
    }

    public class and_attempting_to_add_and_read_from_the_database : when_using_the_bank_account_repository
    {
        Transfer _result;
        Transfer _expected;
        Transfer readAfterDelete;

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
            //Bank b = new Bank() { Name = Guid.NewGuid().ToString() };
            BankAccount a = new BankAccount() { BankAccountId = 1 };
            _expected = new Transfer() { Name = Guid.NewGuid().ToString(), 
                                        FromBankAccount=a, 
                                        Amount = 123, 
                                        AmountTolerence = 0.1M, 
                                        StartDate = DateTime.Parse("2014-01-01"),
                                        Frequency="Month",
                                        IsEnabled=true
            };
        }

        protected override void Because_of()
        {
            //_bankRepository.Add(_expected.Bank);
            int id = _TransferRepository.Add(_expected);
            this._result = _TransferRepository.Read(id);
            _TransferRepository.Delete(new Transfer() { TransferId=id });
            this.readAfterDelete = _TransferRepository.Read(id);

        }

        [Test]
        public void then_the_values_should_be_equal()
        {
            _result.TransferId.ShouldEqual(_expected.TransferId);
            _result.Name.ShouldEqual(_expected.Name);
            _result.StartDate.ShouldEqual(_expected.StartDate);
        }


        [Test]
        public void then_should_be_removed_after()
        {
            this.readAfterDelete.ShouldBeNull();
        }

    }
}
