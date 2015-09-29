using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Finances.Core;

namespace Finances.IntegrationTests.MS
{
    public class ConnectionInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IConnection>().ImplementedBy<IntegrationConnection>());

        }
    }
}
