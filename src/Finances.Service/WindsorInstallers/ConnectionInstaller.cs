using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Finances.Interface;

namespace Finances.Service.WindsorInstallers
{
    public class ConnectionInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IConnection>().ImplementedBy<Connection>());
        }
    }
}
