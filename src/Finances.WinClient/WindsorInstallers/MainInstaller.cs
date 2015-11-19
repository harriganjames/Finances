using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Facilities.TypedFactory;

using Finances.WinClient.ViewModels;
using Finances.Core.Wpf;
using Finances.WinClient.Factories;
using Finances.Core.Interfaces;

namespace Finances.WinClient.WindsorInstallers
{
    public class MainInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            IAppSettings appSettings = container.Resolve<IAppSettings>();

            bool debug;
            bool.TryParse(appSettings.GetSetting("debug"),out debug);


            if (debug)
            {
                // Dashboard
                container.Register(Component.For<IWorkspace>()
                    .ImplementedBy<DashboardViewModel>()
                    );
            }

            // Main
            container.Register(Component.For<IMainViewModel>()
                .ImplementedBy<MainViewModel>()
                );


        }
    }
}
