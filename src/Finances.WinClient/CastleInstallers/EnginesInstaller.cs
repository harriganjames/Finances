using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Finances.Core.Interfaces;
using Finances.Core.Engines;
using Finances.Core.Engines.Cashflow;

namespace Finances.WinClient.CastleInstallers
{
    public class EnginesInstaller : IWindsorInstaller
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
            //container.Register(Component.For<IAggregatedProjectionItemsGeneratorFactory>()
            //            .ImplementedBy<AggregatedProjectionItemsGeneratorFactory>());

            container.Register(Component.For<IScheduleFrequencyCalculator>().ImplementedBy<ScheduleFrequencyCalculatorMonthly>());
            container.Register(Component.For<IScheduleFrequencyCalculator>().ImplementedBy<ScheduleFrequencyCalculatorWeekly>());
            container.Register(Component.For<IScheduleFrequencyCalculatorFactory>().ImplementedBy<ScheduleFrequencyCalculatorFactory>());

            container.Register(Component.For<IProjectionTransferGenerator>().ImplementedBy<ProjectionTransferGenerator>());
            container.Register(Component.For<ITransferDirectionGenerator>().ImplementedBy<TransferDirectionGenerator>());

            
        }
    }
}
