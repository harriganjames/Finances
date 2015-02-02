//using System;
//using System.Collections.Generic;
//using Finances.Core.Entities;
//using Finances.Core.Interfaces;
//using Finances.WinClient.DomainServices;
//using Finances.WinClient.ViewModels;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;

//namespace Finances.UnitTests.MS
//{
//    public abstract class TransferServiceTests
//    {
//        protected ITransferService transferService;

//        protected Mock<ITransferRepository> mockTransferRepository;
//        protected Mock<IBankAccountService> mockBankAccountService;

//        public virtual void Initialize()
//        {
//            mockTransferRepository = new Mock<ITransferRepository>();
//            mockBankAccountService = new Mock<IBankAccountService>();

//            //mockBankAccountService.Setup(s => s.CreateBankAccountItemViewModel()).Returns(()=>
//            //    {
//            //        return new BankAccountItemViewModel() { Bank = new BankItemViewModel() };
//            //    });

//            transferService = new TransferService(mockTransferRepository.Object);
//        }

//    }


//    [TestClass]
//    public class transfer_service_read : TransferServiceTests
//    {
//        int testId = 1;
//        ITransferItemViewModel testObject;
//        Transfer transfer;


//        [TestInitialize]
//        public override void Initialize()
//        {
            
//            base.Initialize();

//            transfer = new Transfer()
//            {
//                TransferId = testId,
//                FromBankAccount = new BankAccount()
//                {
//                    Bank = new Bank()
//                    {
//                        Name = "FMV"
//                    },
//                    Name = "FredMerVer"
//                },
//                ToBankAccount = new BankAccount()
//                {
//                    Bank = new Bank()
//                    {
//                        Name = "T1"
//                    },
//                    Name = "TinMik"
//                },
//                Name = "xfer1"
//            };

//            mockTransferRepository.Setup(s => s.Read(testId)).Returns(transfer);

//            testObject = new TransferItemViewModel();// transferService.CreateTransferViewItemModel();

//            transferService.Read(testId, testObject);
//        }

//        [TestMethod]
//        public void should_create_a_new_VM()
//        {
//            Assert.IsNotNull(testObject);
//        }


//        [TestMethod]
//        public void should_return_valid_properties()
//        {
//            Assert.AreEqual(testObject.TransferId, testId);
//            Assert.AreEqual(testObject.Name, transfer.Name);
//            Assert.AreEqual(testObject.FromBankAccount.AccountName, transfer.FromBankAccount.Name);
//            Assert.AreEqual(testObject.FromBankAccount.Bank.Name, transfer.FromBankAccount.Bank.Name);
//            Assert.AreEqual(testObject.ToBankAccount.AccountName, transfer.ToBankAccount.Name);
//            Assert.AreEqual(testObject.ToBankAccount.Bank.Name, transfer.ToBankAccount.Bank.Name); 
//        }


//    }


//    [TestClass]
//    public class transfer_service_read_list : TransferServiceTests
//    {
//        int testId = 1;
//        List<ITransferItemViewModel> testObject;
//        List<Transfer> transfers;


//        [TestInitialize]
//        public override void Initialize()
//        {

//            base.Initialize();

//            transfers = new List<Transfer>()
//            {
//                new Transfer()
//                {
//                    TransferId = 1,
//                    FromBankAccount = new BankAccount()
//                    {
//                        Bank = new Bank()
//                        {
//                            Name = "FMV"
//                        },
//                        Name = "FredMerVer"
//                    },
//                    ToBankAccount = new BankAccount()
//                    {
//                        Bank = new Bank()
//                        {
//                            Name = "T1"
//                        },
//                        Name = "TinMik"
//                    },
//                    Name = "xfer1"
//                },
//                new Transfer()
//                {
//                    TransferId = 2,
//                    FromBankAccount = new BankAccount()
//                    {
//                        Bank = new Bank()
//                        {
//                            Name = "B1"
//                        },
//                        Name = "A1"
//                    },
//                    ToBankAccount = new BankAccount()
//                    {
//                        Bank = new Bank()
//                        {
//                            Name = "B1"
//                        },
//                        Name = "A2"
//                    },
//                    Name = "xfer2"
//                }

//            };

//            mockTransferRepository.Setup(s => s.ReadList()).Returns(transfers);

//            testObject = transferService.ReadList();
//        }

//        [TestMethod]
//        public void should_create_a_new_list_of_VM()
//        {
//            Assert.IsNotNull(testObject);
//        }

//        [TestMethod]
//        public void should_return_correct_quantity()
//        {
//            Assert.AreEqual(transfers.Count, testObject.Count);
//        }

//        [TestMethod]
//        public void should_return_valid_properties()
//        {
//            for (int i = 0; i < transfers.Count; i++)
//            {
//                Assert.AreEqual(testObject[i].TransferId, transfers[i].TransferId);
//                Assert.AreEqual(testObject[i].Name, transfers[i].Name);
//                Assert.AreEqual(testObject[i].FromBankAccount.AccountName, transfers[i].FromBankAccount.Name);
//                Assert.AreEqual(testObject[i].FromBankAccount.Bank.Name, transfers[i].FromBankAccount.Bank.Name);
//                Assert.AreEqual(testObject[i].ToBankAccount.AccountName, transfers[i].ToBankAccount.Name);
//                Assert.AreEqual(testObject[i].ToBankAccount.Bank.Name, transfers[i].ToBankAccount.Bank.Name);
//            }
//        }


//    }


//}
