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
            // Engine A
            container.Register(Component.For<ICashflowEngineA>().ImplementedBy<CashflowEngineA>());

            // Engine B
            container.Register(Component.For<ICashflowEngineFactoryB>().ImplementedBy<CashflowEngineFactoryB>());

            // Engine C
            container.Register(Component.For<ICashflowEngineC>().ImplementedBy<CashflowEngineC>());

            container.Register(Component.For<IAggregatedProjectionItemsGenerator>().ImplementedBy<AggregatedProjectionItemsGeneratorDetail>());
            container.Register(Component.For<IAggregatedProjectionItemsGenerator>().ImplementedBy<AggregatedProjectionItemsGeneratorMonthlySummary>());
            container.Register(Component.For<IAggregatedProjectionItemsGeneratorFactory>()
                        .ImplementedBy<AggregatedProjectionItemsGeneratorFactory>());

            container.Register(Component.For<ITransferFrequencyDateCalculator>().ImplementedBy<TransferFrequencyDateCalculatorMonthly>());
            container.Register(Component.For<ITransferFrequencyDateCalculatorFactory>().ImplementedBy<TransferFrequencyDateCalculatorFactory>());


        }
    }
}
