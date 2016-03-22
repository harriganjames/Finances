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
using Finances.Core.ValueObjects;
using System.Threading.Tasks;

namespace Finances.UnitTests.MS.Core.EntityTests.CashflowTests
{
    [TestClass]
    public class CashflowGenerateProjectionBasicTest
    {
        Cashflow sut;
        //IBankAccountRepository fakeBankAccountRepository;

        //Mock<IScheduleFrequencyCalculatorFactory> mockTransferFrequencyDateCalculatorFactory;
        //Mock<ITransferDirectionGenerator> mockTransferDirectionGenerator;
        //Mock<IScheduleFrequencyCalculator> mockTransferFrequencyDateCalculatorMonthly;

        //IEnumerable<IScheduleFrequencyCalculator> scheduleFrequencyCalculators;

        Mock<ICashflowProjectionTransferGenerator> mockIProjectionTransferGenerator;


        [TestInitialize]
        public void Initialize()
        {
            //IBankAccountRepository bankAccountRepo;
            //ITransferRepository transferRepo;


            List<CashflowProjectionTransfer> cashflowProjectionTransfers = null;

            //mockIProjectionTransferGenerator = new Mock<IProjectionTransferGenerator>();


            mockIProjectionTransferGenerator
                .Setup(s => s.GenerateCashflowProjectionTransfersAsync(
                            It.IsAny<List<CashflowBankAccount>>(),
                            It.IsAny<DateTime>(),
                            It.IsAny<DateTime>()
                            )
                )
                .Returns(Task.Factory.StartNew(()=>cashflowProjectionTransfers));

            //fakeBankAccountRepository = new FakeBankAccountRepository();

            //mockTransferFrequencyDateCalculatorFactory = new Mock<IScheduleFrequencyCalculatorFactory>();
            //mockTransferDirectionGenerator = new Mock<ITransferDirectionGenerator>();

            //mockTransferFrequencyDateCalculatorMonthly = new Mock<IScheduleFrequencyCalculator>();

            //mockTransferFrequencyDateCalculatorMonthly.Setup(s => s.CalculateNextDate(It.IsAny<Schedule>(), It.IsAny<DateTime>()))
            //    .Returns((Transfer t, DateTime d) => d.AddMonths(1));


            sut = new Cashflow(new CashflowProjection(mockIProjectionTransferGenerator.Object,null))
            {
                OpeningBalance = 5000,
                StartDate = new DateTime(2015, 08, 1),
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
        public void TestGenerateProjectionBasic()
        {
            var mode = new CashflowProjectionModeMonthlySummary();

            var cpi = sut.GenerateProjectionAsync(new DateTime(2015, 01, 01), new DateTime(2015, 12, 31), 10000, 5000, mode).Result;


            //CashflowProjectionItems = cashflowEngineC.GenerateProjection(SelectedCashflow.Entity.CashflowBankAccounts,
            //        SelectedCashflow.Entity.StartDate,
            //        SelectedCashflow.Entity.StartDate.AddMonths(Decimal.ToInt32(QtyMonths.Value)),
            //        OpeningBalance.Value, Threshold.Value, SelectedMode);



        }
    }
}
