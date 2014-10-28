using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Finances.Core.Wpf;
using Finances.Core.Wpf.ObjectInformation;
using System.Windows;
using System.Windows.Threading;
using Finances.Core;

namespace Finances.WinClient.CastleInstallers
{
    public class UtilitiesInstaller :IWindsorInstaller
    {
        readonly Window wpfWindow;

        public UtilitiesInstaller(Window wpfWindow)
        {
            this.wpfWindow = wpfWindow;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<Dispatcher>().UsingFactoryMethod(() => wpfWindow.Dispatcher));

            container.Register(Component.For<ObjectInformation>().ImplementedBy<ObjectInformation>());

            container.Register(Component.For<ObjectInfoViewModel>().ImplementedBy<ObjectInfoViewModel>());

            container.Register(Component.For<Apex>().ImplementedBy<Apex>());
        }
    }
}
