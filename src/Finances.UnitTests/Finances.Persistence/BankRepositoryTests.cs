using NUnit.Framework;
using NBehave.Spec.NUnit;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Persistence.FNH;
using System;
using Moq;
using NHibernate;
using System.Collections.Generic;

namespace Finances.UnitTests.DONOTUSE.Finances.Persistence.BankRepositoryTests
{
    public class when_working_with_the_bank_repository : Specification
    {
        protected IBankRepository bankRepository;
        protected Mock<ISessionFactory> sessionFactory;
        protected Mock<ISession> session;

        protected override void Establish_context()
        {
            base.Establish_context();

            sessionFactory = new Mock<ISessionFactory>();
            session = new Mock<ISession>();

            sessionFactory.Setup(sf => sf.OpenSession()).Returns(session.Object);

            bankRepository = new BankRepository(sessionFactory.Object);
        }
    }

    ///////////////////////////////////////


    ///// Valid Banks /////

    public class and_adding_a_valid_bank : when_working_with_the_bank_repository
    {
        private int result;
        protected Bank testBank;
        protected int bankId;

        protected override void Establish_context()
        {
            base.Establish_context();

            var randomNumberGenerator = new Random();
            bankId = randomNumberGenerator.Next(32000);

            base.session.Setup(s => s.Save(testBank)).Returns(bankId);
            base.session.Setup(s => s.Update(testBank));
        }

        protected override void Because_of()
        {
            result = bankRepository.Add(testBank);
        }

        [Test]
        public void then_a_valid_bank_id_should_be_returned()
        {
            result.ShouldEqual(bankId);
        }
    }


    public class and_updating_a_valid_bank : when_working_with_the_bank_repository
    {
        private bool result;
        Bank testBank;
        Bank updatedBank;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBank = new Bank() { BankId = 1, Name = "test" };

            base.session.Setup(s => s.Update(testBank)).Callback<Object>(b => { updatedBank = b as Bank; });
        }

        protected override void Because_of()
        {
            result = bankRepository.Update(testBank);
        }

        [Test]
        public void then_a_bank_should_be_updated()
        {
            result.ShouldEqual(true);
            updatedBank.ShouldEqual(testBank);
        }
    }


    public class and_deleting_a_valid_bank : when_working_with_the_bank_repository
    {
        private bool result;
        int testBankId;
        Bank deletedBank;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBankId = 123;

            base.session.Setup(s => s.Delete(It.IsAny<Bank>())).Callback<Object>(b => { deletedBank = (Bank)b; });
        }

        protected override void Because_of()
        {
            result = bankRepository.Delete(new Bank() { BankId = testBankId });
        }

        [Test]
        public void then_a_bank_should_be_deleted()
        {
            result.ShouldEqual(true);
            deletedBank.BankId.ShouldEqual(testBankId);
        }
    }


    public class and_reading_a_valid_bank : when_working_with_the_bank_repository
    {
        Bank result;
        Bank testBank;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBank = new Bank() { BankId = 1, Name = "test" };

            base.session.Setup(s => s.Get<Bank>(testBank.BankId)).Returns(testBank);
        }

        protected override void Because_of()
        {
            result = bankRepository.Read(testBank.BankId);
        }

        [Test]
        public void then_a_valid_bank_should_be_returned()
        {
            result.ShouldEqual(testBank);
        }
    }

    public class and_reading_a_list_of_valid_banks : when_working_with_the_bank_repository
    {
        List<Bank> result;
        List<Bank> testBanks;
        Mock<ICriteria> criteria;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBanks = new List<Bank>() {
                new Bank() { BankId = 1, Name = "test1" }
                ,new Bank() { BankId = 2, Name = "test2" } };

            criteria = new Mock<ICriteria>();

            criteria.Setup(c => c.List<Bank>()).Returns(testBanks);

            base.session.Setup(s => s.CreateCriteria<Bank>()).Returns(criteria.Object);
                
        }

        protected override void Because_of()
        {
            result = bankRepository.ReadList();
        }

        [Test]
        public void then_a_valid_list_of_banks_should_be_returned()
        {
            result.ShouldEqual(testBanks);
        }
    }


    public class and_reading_a_list_of_valid_DataIdNames : when_working_with_the_bank_repository
    {
        List<DataIdName> result;
        List<DataIdName> testData = new List<DataIdName>();
        List<Bank> testBanks;
        Mock<ICriteria> criteria;

        protected override void Establish_context()
        {
            base.Establish_context();

            testBanks = new List<Bank>() {
                new Bank() { BankId = 1, Name = "test1" }
                ,new Bank() { BankId = 2, Name = "test2" } };


            testBanks.ForEach(b => {
                    testData.Add(new DataIdName() { Id=b.BankId, Name=b.Name });
                });

            criteria = new Mock<ICriteria>();

            criteria.Setup(c => c.List<Bank>()).Returns(testBanks);

            base.session.Setup(s => s.CreateCriteria<Bank>()).Returns(criteria.Object);

        }

        protected override void Because_of()
        {
            result = bankRepository.ReadListDataIdName();
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

    public class and_adding_an_invalid_bank : when_working_with_the_bank_repository
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
                this.bankRepository.Add(null);
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


    public class and_updating_an_invalid_bank : when_working_with_the_bank_repository
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
                this.bankRepository.Update(null);
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
