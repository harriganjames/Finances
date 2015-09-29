using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Finances.Core;
using Finances.Core.Interfaces;

namespace Finances.WinClient.CastleInstallers 
{
    public class SystemInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IAppSettings>().ImplementedBy<AppSettings>());
        }
    }
}
