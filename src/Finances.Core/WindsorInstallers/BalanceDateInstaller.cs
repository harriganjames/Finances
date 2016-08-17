using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Facilities.TypedFactory;

using Finances.Core.Factories;
using Finances.Core.Entities;

namespace Finances.Core.WindsorInstallers
{
    public class BalanceDateInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Component.For<BalanceDate>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<IBalanceDateFactory>()
                .AsFactory()
                );
        }
    }
}
