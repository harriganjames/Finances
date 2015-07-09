using System;
using System.Linq;
using System.Collections.Generic;
using Finances.Core.Engines;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Finances.Core.Engines.Cashflow;

namespace Finances.UnitTests.MS
{
    [TestClass]
    public class CashflowEngineTests
    {
        ICashflowEngineA cashflowEngine;

        Mock<IBankAccountRepository> bankAccountRepository;
        Mock<ITransferRepository> transferRepository;

        List<BankAccount> bankAccountList = new List<BankAccount>();
        List<Transfer> transferList = new List<Transfer>();


        [TestInitialize]
        public void Initialize()
        {
            bankAccountRepository = new Mock<IBankAccountRepository>();
            transferRepository = new Mock<ITransferRepository>();

            cashflowEngine = new CashflowEngineA(bankAccountRepository.Object, transferRepository.Object);

            bankAccountRepository.Setup(s => s.ReadList()).Returns(bankAccountList);
            transferRepository.Setup(s => s.ReadList()).Returns(transferList);
        
        }



        [TestMethod]
        public void TestGenerateCashflowProjection()
        {
            bankAccountList.AddRange(new []
            {
                new BankAccount()
                {
                    BankAccountId=1
                },
                new BankAccount()
                {
                    BankAccountId=2
                }
            }
            );

            transferList.AddRange(new []
            {
                new Transfer()
                {
                    Name = "1 -> 2 - internal",
                    Amount = 10,
                    IsEnabled = true,
                    StartDate = DateTime.Parse("2015-04-01"),
                    Frequency = "Monthly",
                    FromBankAccount = bankAccountList.Single(a=>a.BankAccountId==1),
                    ToBankAccount = bankAccountList.Single(a=>a.BankAccountId==2)
                },
                new Transfer()
                {
                    Name = "Elswhere -> 2 - inbound",
                    Amount = 20,
                    IsEnabled = true,
                    StartDate = DateTime.Parse("2015-04-01"),
                    Frequency = "Monthly",
                    FromBankAccount = null,
                    ToBankAccount = bankAccountList.Single(a=>a.BankAccountId==2)
                },
                new Transfer()
                {
                    Name = "1 -> Elsewhere - outbound",
                    Amount = 30,
                    IsEnabled = true,
                    StartDate = DateTime.Parse("2015-04-01"),
                    Frequency = "Monthly",
                    FromBankAccount = bankAccountList.Single(a=>a.BankAccountId==1),
                    ToBankAccount = null
                },

            });

            Cashflow cf = new Cashflow()
            {
                CashflowId = 1,
                OpeningBalance = 100,
                StartDate = DateTime.Parse("2015-04-01")
            };


            cf.CashflowBankAccounts = new List<CashflowBankAccount>();


            var cpis = cashflowEngine.GenerateProjection(cf.CashflowBankAccounts, new DateTime(2015, 4, 1), new DateTime(2015, 10, 1), 123, 100, ProjectionModeEnum.Detail);


        }
    }
}
