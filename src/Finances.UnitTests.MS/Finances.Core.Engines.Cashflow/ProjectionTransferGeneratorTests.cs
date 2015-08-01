using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

using Finances.UnitTests.MS.Fakes;

using Moq;
using Finances.Core.Engines.Cashflow;
using Finances.Core.Interfaces;
using Finances.Core.Entities;
using System.Collections.Generic;

namespace Finances.UnitTests.MS.Finances.Core.Engines.Cashflow
{
    [TestClass]
    public class ProjectionTransferGeneratorTests
    {
        IProjectionTransferGenerator sut;

        IBankAccountRepository fakeBankAccountRepository;

        Mock<ITransferFrequencyDateCalculatorFactory> mockTransferFrequencyDateCalculatorFactory;
        Mock<ITransferDirectionGenerator> mockTransferDirectionGenerator;
        Mock<ITransferFrequencyDateCalculator> mockTransferFrequencyDateCalculatorMonthly;

        DateTime cashflowStartDate;
        DateTime cashflowEndDate;
        List<CashflowBankAccount> allAccounts;

        [TestInitialize]
        public void Initialize()
        {
            fakeBankAccountRepository = new FakeBankAccountRepository();

            mockTransferFrequencyDateCalculatorFactory = new Mock<ITransferFrequencyDateCalculatorFactory>();
            mockTransferDirectionGenerator = new Mock<ITransferDirectionGenerator>();

            mockTransferFrequencyDateCalculatorMonthly = new Mock<ITransferFrequencyDateCalculator>();

            mockTransferFrequencyDateCalculatorMonthly.Setup(s => s.CalculateNextDate(It.IsAny<Transfer>(), It.IsAny<DateTime>()))
                .Returns((Transfer t, DateTime d) => d.AddMonths(1));


            allAccounts = new List<CashflowBankAccount>();

            sut = new ProjectionTransferGenerator(
                                    fakeBankAccountRepository,
                                    mockTransferFrequencyDateCalculatorFactory.Object,
                                    mockTransferDirectionGenerator.Object
                                    );

        }
        // scenarioes:
        // CF startDate, CF endDate, transferDate, fromAcc, toAcc 
        //  5           10          
        //
        //
        //
        [TestMethod]
        public void TestSingleOccurranceTransferWithinDateRange()
        {
            List<CashflowProjectionTransfer> results;

            mockTransferFrequencyDateCalculatorFactory.Setup(s => s.Create(It.IsAny<Transfer>()))
                .Returns(mockTransferFrequencyDateCalculatorMonthly.Object);

            var testdata = new[] 
            { 
                new 
                {   // all accounts, from elsewhere 
                    TransferDate = new DateTime(2015,7,10), 
                    StartDate = new DateTime(2015,7,5), 
                    EndDate = new DateTime(2015,10,20),
                    FromAccount = (BankAccount)null,
                    ToAccount = new BankAccount() { BankAccountId=2 },
                    CashflowAccounts = new List<CashflowBankAccount>(),
                    QtyInbound = 1,
                    QtyOutbound = 0,
                    Message = "All accounts. From elsewhere. In=1, Out=0"
                },
                new 
                {   // all accounts, from elsewhere 
                    TransferDate = new DateTime(2015,7,10), 
                    StartDate = new DateTime(2015,7,5), 
                    EndDate = new DateTime(2015,10,20),
                    FromAccount = new BankAccount() { BankAccountId=2 },
                    ToAccount = (BankAccount)null,
                    CashflowAccounts = new List<CashflowBankAccount>(),
                    QtyInbound = 0,
                    QtyOutbound = 1,
                    Message = "All accounts. From elsewhere. In=0, Out=1"
                }
            };


            foreach (var data in testdata)
            {
                var bankAccounts = new List<BankAccount>();
                foreach (var cba in data.CashflowAccounts)
	            {
                    bankAccounts.Add(cba.BankAccount);
	            }


                var tds = new List<TransferDirection>() 
                { 
                    new TransferDirection()
                    {
                        IsInbound = data.QtyInbound>0,
                        IsOutbound = data.QtyOutbound>0,
                        Transfer = new Transfer()
                        {
                            StartDate = data.TransferDate,
                            EndDate = data.TransferDate,
                            FromBankAccount = data.FromAccount,
                            ToBankAccount = data.ToAccount,
                            IsEnabled = true,
                            Amount = 100
                        }
                    }
                };

                mockTransferDirectionGenerator.Setup(s => s.GetTransferDirections(It.IsAny<List<BankAccount>>()))
                    .Returns(tds);


                results = sut.GenerateProjectionTransfers(data.CashflowAccounts, data.StartDate, data.EndDate);

                Assert.AreEqual(results.Count(c => c.TransferDirection.IsInbound), data.QtyInbound, data.Message);

                Assert.AreEqual(results.Count(c => c.TransferDirection.IsOutbound), data.QtyOutbound, data.Message);

            
            }





        }
    }
}
