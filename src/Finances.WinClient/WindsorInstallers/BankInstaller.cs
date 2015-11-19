using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Facilities.TypedFactory;

using Finances.WinClient.DomainServices;
using Finances.WinClient.ViewModels;
//using Finances.WinClient.InterceptorSelectors;
using Finances.Core.Interfaces;
using Finances.Core.Wpf;
using Finances.WinClient.Factories;

namespace Finances.WinClient.WindsorInstallers
{
    public class BankInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            IAppSettings appSettings = container.Resolve<IAppSettings>();

            bool debug;
            bool.TryParse(appSettings.GetSetting("debug"), out debug);

            // Banks
            container.Register(Component.For<IWorkspace>()
                .Forward<IBankListViewModel>()
                .ImplementedBy<BankListViewModel>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<BankEditorViewModel>()
                .ImplementedBy<BankEditorViewModel>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<IBankEditorViewModelFactory>()
                .AsFactory()
                );

            if (debug)
            {
                // Bank Tree
                container.Register(Component.For<IWorkspace>().ImplementedBy<BankTreeViewModel>());
            }


            container.Register(Component.For<IBankAgent>().ImplementedBy<BankAgent>());


        }
    }
}
