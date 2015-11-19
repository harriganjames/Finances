using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Facilities.TypedFactory;

using Finances.WinClient.ViewModels;
using Finances.Core.Wpf;
using Finances.WinClient.Factories;
using Finances.Core.Interfaces;
using Finances.WinClient.DomainServices;

namespace Finances.WinClient.WindsorInstallers
{
    public class TransferInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            IAppSettings appSettings = container.Resolve<IAppSettings>();

            bool debug;
            bool.TryParse(appSettings.GetSetting("debug"),out debug);

            container.Register(Component.For<IWorkspace>()
                .Forward<ITransferListViewModel>() // for dashboard
                .ImplementedBy<TransferListViewModel>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<TransferEditorViewModel>()
                .ImplementedBy<TransferEditorViewModel>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<ITransferEditorViewModelFactory>()
                .AsFactory()
                );


            container.Register(Component.For<ITransferAgent>().ImplementedBy<TransferAgent>());

        }
    }
}
