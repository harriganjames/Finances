using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using System.Windows;
using Finances.Service;

namespace Finances.Service.WindsorInstallers
{
    public class UtilitiesInstaller :IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<Apex>().ImplementedBy<Apex>());
        }
    }
}
