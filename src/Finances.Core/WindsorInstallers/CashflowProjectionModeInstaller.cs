using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Finances.Core.Interfaces;
using Finances.Core.Engines.Cashflow;

namespace Finances.Core.WindsorInstallers
{
    public class CashflowProjectionModeInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ICashflowProjectionMode>()
                .ImplementedBy<CashflowProjectionModeDetail>());
            container.Register(Component.For<ICashflowProjectionMode>()
                .ImplementedBy<CashflowProjectionModeMonthlySummary>());

        }
    }
}
