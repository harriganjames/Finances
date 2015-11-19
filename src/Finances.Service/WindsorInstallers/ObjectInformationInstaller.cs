using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using System.Windows;
using Finances.Service;

namespace Finances.WinClient.CastleInstallers
{
    public class ObjectInformationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ObjectInformation>().ImplementedBy<ObjectInformation>());
        }
    }
}
