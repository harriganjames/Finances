using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Finances.Core.Interfaces;
using Finances.Core.Engines;
using Finances.Core.Engines.Cashflow;

namespace Finances.WinClient.CastleInstallers
{
    class EnginesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ICashflowEngineA>().ImplementedBy<CashflowEngineA>());

            container.Register(Component.For<ICashflowEngineFactoryB>().ImplementedBy<CashflowEngineFactoryB>());

            container.Register(Component.For<ICashflowEngineC>().ImplementedBy<CashflowEngineC>());
            container.Register(Component.For<IAggregatedProjectionItemsGenerator>().ImplementedBy<DetailAggregatedProjectionItemsGenerator>());
            container.Register(Component.For<IAggregatedProjectionItemsGenerator>().ImplementedBy<MonthlySummaryAggregatedProjectionItemsGenerator>());
     
        }
    }
}
