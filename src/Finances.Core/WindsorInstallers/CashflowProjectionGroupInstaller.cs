using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Facilities.TypedFactory;

using Finances.Core.Factories;
using Finances.Core.Entities;
using Finances.Core.Engines.Cashflow;

namespace Finances.Core.WindsorInstallers
{
    public class CashflowProjectionGroupInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Component.For<CashflowProjectionGroup>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<ICashflowProjectionGroupFactory>()
                .AsFactory()
                );
        }
    }
}
