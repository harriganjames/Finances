using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Finances.Core.Interfaces;
using Finances.Core.Engines.Cashflow;

namespace Finances.Core.WindsorInstallers
{
    public class CashflowProjectionInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ICashflowProjectionTransferGenerator>()
                .ImplementedBy<CashflowProjectionTransferGenerator>());
            container.Register(Component.For<ITransferDirectionGenerator>()
                .ImplementedBy<TransferDirectionGenerator>());

            container.Register(Component.For<ICashflowProjection>()
                .ImplementedBy<CashflowProjection>());

        }
    }
}
