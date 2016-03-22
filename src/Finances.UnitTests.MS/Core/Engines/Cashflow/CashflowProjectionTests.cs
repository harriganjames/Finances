//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Finances.Core.Interfaces;
//using Finances.Core.Engines.Cashflow;
//using Finances.UnitTests.MS.Fakes;
//using Moq;

//namespace Finances.UnitTests.MS.Core.Engines.Cashflow
//{
//    [TestClass]
//    public class CashflowProjectionTests
//    {
//        ICashflowProjection sut;
//        ICashflowProjectionTransferGenerator cptg;
//        IBankAccountRepository fakeBankAccountRepository;
//        Mock<ITransferDirectionGenerator> mockTransferDirectionGenerator;

//        [TestInitialize]
//        public void Initialize()
//        {
//            fakeBankAccountRepository = new FakeBankAccountRepository();
//            mockTransferDirectionGenerator = new Mock<ITransferDirectionGenerator>();

//            cptg = new CashflowProjectionTransferGenerator(fakeBankAccountRepository, mockTransferDirectionGenerator.Object);


//            sut = new CashflowProjection()
//        }



//        [TestMethod]
//        public void TestMethod1()
//        {
//            mockTransferDirectionGenerator.Setup(s => s.GetTransferDirections(It.IsAny<List<BankAccount>>()))
//    .Returns(tds);



//        }
//    }
//}
