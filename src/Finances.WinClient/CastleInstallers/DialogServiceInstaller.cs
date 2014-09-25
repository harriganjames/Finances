using System.Windows;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Finances.Core.Wpf;

namespace Finances.WinClient.CastleInstallers
{
    class DialogServiceInstaller : IWindsorInstaller
    {
        readonly Window wpfWindow;

        public DialogServiceInstaller(Window wpfWindow)
        {
            this.wpfWindow = wpfWindow;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IDialogService>().UsingFactoryMethod(() => new DialogService(wpfWindow)));
        }
    }
}
