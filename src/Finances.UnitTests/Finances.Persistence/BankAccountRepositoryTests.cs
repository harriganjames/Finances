using NUnit.Framework;
using NBehave.Spec.NUnit;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Persistence.FNH;
using System;
using Moq;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Finances.UnitTests.DONOTUSE.Finances.Persistence.BankAccountRepositoryTests
{
    public class when_working_with_the_bank_account_repository : Specification
    {
        protected IBankAccountRepository bankAccountRepository;
        protected Mock<ISessionFactory> sessionFactory;
        protected Mock<ISession> session;

        protected override void Establish_context()
        {
            base.Establish_context();

            sessionFactory = new Mock<ISessionFactory>();
            session = new Mock<ISession>(MockBehavior.Strict);
            session.Setup(s => s.Dispose());
            session.Setup(s => s.GetSessionImplementation());

            sessionFactory.Setup(sf => sf.OpenSession()).Returns(session.Object);

            bankAccountRepository = new BankAccountRepository(sessionFactory.Object);
        }
    }

    ///////////////////////////////////////


    ///// Valid Banks /////

    public class and_adding_a_valid_bank_account : when_working_with_the_bank_account_repository
    {
        private int result;
        protected BankAccount testBankAccount;
        protected int bankAccountId;

        protected override void Establish_context()
        {
            base.Establish_context();

            var randomNumberGenerator = new Random();
            bankAccountId = randomNumberGenerator.Next(32000);

            base.session.Setup(s => s.Save(testBankAccount)).Returns(bankAccountId);
            base.session.Setup(s => s.Update(testBankAccount));
        }

        protected override void Because_of()
        {
            result = bankAccountRepository.Add(testBankAccount);
        }

        [Test]
        public void then_a_valid_bank_account_id_should_be_returned()
        {
            result.ShouldEqual(bankAccountId);
        }
    }

    public class and_updating_a_valid_bank_account : when_working_with_the_bank_account_repository
    {
        private bool result;
        BankAccount testBankAccount;
        BankAccount updatedBankAccount;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBankAccount = new BankAccount() { BankAccountId = 1, Name = "test" };

            base.session.Setup(s => s.Update(testBankAccount)).Callback<Object>(a => { updatedBankAccount = a as BankAccount; });
        }

        protected override void Because_of()
        {
            result = bankAccountRepository.Update(testBankAccount);
        }

        [Test]
        public void then_a_bank_account_should_be_updated()
        {
            result.ShouldEqual(true);
            updatedBankAccount.ShouldEqual(testBankAccount);
        }
    }

    public class and_deleting_a_valid_bank_account : when_working_with_the_bank_account_repository
    {
        private bool result;
        int testBankAccountId;
        BankAccount deletedBankAccount;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBankAccountId = 123;

            base.session.Setup(s => s.Delete(It.IsAny<BankAccount>())).Callback<Object>(b => { deletedBankAccount = (BankAccount)b; });
        }

        protected override void Because_of()
        {
            result = bankAccountRepository.Delete(testBankAccountId);
        }

        [Test]
        public void then_a_bank_account_should_be_deleted()
        {
            result.ShouldEqual(true);
            deletedBankAccount.BankAccountId.ShouldEqual(testBankAccountId);
        }
    }


    public class and_reading_a_valid_bank_account : when_working_with_the_bank_account_repository
    {
        BankAccount result;
        BankAccount testBankAccount;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBankAccount = new BankAccount() { BankAccountId = 1, Name = "test", Bank = new Bank() { BankId = 1, Name = "HSBC" } };

            base.session.Setup(s => s.Get<BankAccount>(testBankAccount.BankAccountId)).Returns(testBankAccount);
        }

        protected override void Because_of()
        {
            result = bankAccountRepository.Read(testBankAccount.BankAccountId);
        }

        [Test]
        public void then_a_valid_bank_account_should_be_returned()
        {
            result.ShouldEqual(testBankAccount);
        }

        [Test]
        public void then_it_should_have_a_valid_bank()
        {
            result.Bank.ShouldEqual(testBankAccount.Bank);
        }

        [Test]
        public void then_it_should_have_a_valid_bank_name()
        {
            result.Bank.Name.ShouldEqual(testBankAccount.Bank.Name);
        }
    }

    public class and_reading_a_list_of_valid_bank_accounts : when_working_with_the_bank_account_repository
    {
        List<BankAccount> result;
        List<BankAccount> testBankAccounts;
        Mock<IQueryable<BankAccount>> query;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBankAccounts = new List<BankAccount>() {
                new BankAccount() { BankAccountId = 1, Name = "test1", Bank = new Bank() { BankId = 1, Name = "HSBC1" } }
                ,new BankAccount() { BankAccountId = 2, Name = "test2", Bank = new Bank() { BankId = 1, Name = "HSBC1" } } };

            query = new Mock<IQueryable<BankAccount>>();

            
            //Expression<Func<TOriginating, TRelated>> 
    //        query.Setup(c => c.Fetch(
    //            It.IsAny<
    //                Expression<
    //                    Func<IQueryable<BankAccount>, IQueryable<BankAccount>>
    //                    >
    //                >()
    //            ));

    //        query.Setup(c => c.Fetch(
    //It.IsAny<
    //    Expression<
    //        Func<IQueryable<BankAccount>, IQueryable<BankAccount>>
    //        >
    //    >()
    //));

    //        base.session.Setup(s => s.Query<BankAccount>()).Returns((IQueryable<BankAccount>)testBankAccounts);

        }

        protected override void Because_of()
        {
            result = bankAccountRepository.ReadList();
        }

        [Test]
        public void then_a_valid_list_of_bank_accounts_should_be_returned()
        {
            result.ShouldEqual(testBankAccounts);
        }


    }

    public class and_reading_a_list_of_valid_DataIdNames : when_working_with_the_bank_account_repository
    {
        List<DataIdName> result;
        List<DataIdName> testData = new List<DataIdName>();
        List<BankAccount> testBankAccounts;
        Mock<ICriteria> criteria;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBankAccounts = new List<BankAccount>() {
                new BankAccount() { BankAccountId = 1, Name = "test1", Bank = new Bank() { Name="HSBC" } }
                ,new BankAccount() { BankAccountId = 2, Name = "test2", Bank = new Bank() { Name="HSBC" } } };


            testBankAccounts.ForEach(a =>
            {
                testData.Add(new DataIdName() { Id = a.BankAccountId, Name = string.Format("{0} - {1}", a.Bank.Name, a.Name) });
            });

            criteria = new Mock<ICriteria>();

            criteria.Setup(c => c.List<BankAccount>()).Returns(testBankAccounts);

            base.session.Setup(s => s.CreateCriteria<BankAccount>()).Returns(criteria.Object);

        }

        protected override void Because_of()
        {
            result = bankAccountRepository.ReadListDataIdName();
        }

        [Test]
        public void then_a_valid_list_of_DataIdNames_should_be_returned()
        {
            result.Count.ShouldEqual(testData.Count);

            for (int i = 0; i < result.Count; i++)
            {
                result[i].Id.ShouldEqual(testData[i].Id);
                result[i].Name.ShouldEqual(testData[i].Name);
            }
        }
    }


    ///// Invalid Banks /////

    public class and_adding_a_null_bank_account : when_working_with_the_bank_account_repository
    {
        private Exception result;

        protected override void Establish_context()
        {
            base.Establish_context();

            base.session.Setup(s => s.Save(null)).Throws(new ArgumentNullException());
        }

        protected override void Because_of()
        {
            try
            {
                this.bankAccountRepository.Add(null);
            }
            catch (Exception e)
            {
                result = e;
            }
        }


        [Test]
        public void then_an_argument_null_exception_should_be_raised()
        {
            result.ShouldBeInstanceOfType<ArgumentNullException>();
        }

    }

    public class and_updating_a_null_bank_account : when_working_with_the_bank_account_repository
    {
        private Exception result;

        protected override void Establish_context()
        {
            base.Establish_context();

            base.session.Setup(s => s.Update(null)).Throws(new ArgumentNullException());
        }

        protected override void Because_of()
        {
            try
            {
                this.bankAccountRepository.Update(null);
            }
            catch (Exception e)
            {
                result = e;
            }
        }


        [Test]
        public void then_an_argument_null_exception_should_be_raised()
        {
            result.ShouldBeInstanceOfType<ArgumentNullException>();
        }

    }

}