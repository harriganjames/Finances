using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

using Finances.UnitTests.MS.Fakes;

using Moq;
using Finances.Core.Engines.Cashflow;
using Finances.Core.Interfaces;
using Finances.Core.Entities;
using System.Collections.Generic;
using Finances.Core.Factories;
using Finances.Core;
using Finances.Core.Engines;


namespace Finances.UnitTests.MS.Core.EntityTests.CashflowTests
{
    [TestClass]
    public class CashflowGenerateProjectionTest
    {
        Cashflow sut;
        List<Transfer> transfers;

        IBankAccountRepository fakeBankAccountRepository;
        
        Mock<ITransferRepository> mockTransferRepository = new Mock<ITransferRepository>();

        IScheduleFrequencyCalculator[] scheduleFrequencyCalculators = 
                    new IScheduleFrequencyCalculator[] 
                    { 
                                new ScheduleFrequencyCalculatorMonthly(),
                                new ScheduleFrequencyCalculatorWeekly() 

                    };

        DateTime cashflowStartDate = new DateTime(2015, 08, 1);
        Decimal cashflowOperningBalance = 5000;
        Decimal transferAmount = 100;


        [TestInitialize]
        public void Initialize()
        {
            /*
            * Cashflow(IProjectionTransferGenerator)
            * 
            * ProjectionTransferGenerator(IBankAccountRepository,ITransferDirectionGenerator) : IProjectionTransferGenerator
            * 
            * TransferDirectionGenerator(ITransferRepository) : ITransferDirectionGenerator
            * 
            */

            transfers = new List<Transfer>()
            {
                new Transfer(new FakeScheduleFactory())
                {
                    TransferId=1,
                    FromBankAccount = new BankAccount() { BankAccountId=1 },
                    ToBankAccount = new BankAccount() { BankAccountId=2 },
                    Amount = transferAmount,
                    Category = new TransferCategory() { Code="NONE" },
                    IsEnabled = true,
                    Schedule = new Schedule(scheduleFrequencyCalculators)
                    {
                        StartDate = cashflowStartDate,
                        EndDate = new DateTime(2016,08,01),
                        Frequency = "Monthly",
                        FrequencyEvery = 1
                    }
                }
            };


            mockTransferRepository
                .Setup(s => s.ReadList())
                .Returns(transfers);


            fakeBankAccountRepository = new FakeBankAccountRepository();


            sut = new Cashflow(new ProjectionTransferGenerator(fakeBankAccountRepository,
                                new TransferDirectionGenerator(mockTransferRepository.Object)))
            {
                OpeningBalance = cashflowOperningBalance,
                StartDate = cashflowStartDate,
                CashflowBankAccounts = new List<CashflowBankAccount>()
                {
                    new CashflowBankAccount()
                    {
                        BankAccount = new BankAccount()
                        {
                            BankAccountId=1
                        }
                    }
                }
            };

        }

        [TestMethod]
        public void TestGenerateProjection()
        {
            Decimal openingBalance = 10000;
            int qtyMonths = 6;
            DateTime endDate = cashflowStartDate.AddMonths(qtyMonths);
            var mode = new AggregatedProjectionItemsGeneratorMonthlySummary();

            var cpi = sut.GenerateProjection(endDate, openingBalance, 5000, mode);

            int qtyProjections = qtyMonths + 1;

            Assert.AreEqual(qtyProjections+1, cpi.Count, "Quantity items");

            Decimal balance = openingBalance - (transferAmount * qtyProjections);

            Assert.AreEqual(balance, cpi[cpi.Count - 1].Balance, "Balance");

        }
    }
}
