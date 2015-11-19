using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Facilities.TypedFactory;

using Finances.Core.Factories;
using Finances.Core.Entities;

namespace Finances.Core.WindsorInstallers
{
    public class CashflowInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Component.For<Cashflow>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<ICashflowFactory>()
                .AsFactory()
                );
        }
    }
}
