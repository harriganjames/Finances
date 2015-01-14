using System;
using Finances.Core.Interfaces;
using Finances.Persistence.EF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Finances.WinClient.DomainServices;
using Finances.WinClient.ViewModels;
using Finances.Core.Wpf;
using AutoMapper;

namespace Finances.IntegrationTests.MS
{
    [TestClass]
    public class BankAgentEF
    {
        IBankRepository _repository;
        IBankAgent _agent;


        [TestInitialize]
        public void Initialize()
        {
            string connectionString = "data source=MIKE_LAPTOP;initial catalog=FinanceINT;integrated security=True;App=MSTest;";
            var mcfactory = new ModelContextFactory(connectionString);

            new Finances.Persistence.EF.MappingCreator().CreateMappings();
            _repository = new BankRepository(mcfactory,Mapper.Engine);


            new Finances.WinClient.DomainServices.MappingCreator().CreateMappings();
            _agent = new BankAgent(_repository);


        }


        [TestMethod]
        public void TestCRUD()
        {
            BankEditorViewModel read = new BankEditorViewModel((IBankService)null, (IDialogService)null);
            BankItemViewModel read2 = new BankItemViewModel();
            var b = new BankEditorViewModel((IBankService)null,(IDialogService)null) { Name = "TestCRUD-" + Guid.NewGuid().ToString() };

            _agent.Add(b);
            Assert.IsTrue(b.BankId > 0,"BankId not set");

            _agent.Read(b.BankId, read);
            Assert.AreEqual(read.BankId, b.BankId, "Read test");

            b.Name += "-UPDATE";
            _agent.Update(b);

            _agent.Read(b.BankId, read2);
            Assert.AreEqual(read2.Name, b.Name, "Update test");

            _agent.Delete(read2);

            bool result = _agent.Read(b.BankId, read2);
            Assert.IsFalse(result, "post-delete test");
        }

    }
}
