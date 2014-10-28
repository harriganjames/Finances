using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;

using Finances.WinClient.ViewModels;
using Finances.Core.Wpf;

namespace Finances.WinClient.CastleInstallers
{
    class ViewModelInstallers : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Banks
            container.Register(Component.For<IWorkspace>().ImplementedBy<BanksViewModel>());
            container.Register(Component.For<IBankEditorViewModel>().ImplementedBy<BankEditorViewModel>());
            container.Register(Component.For<IWorkspace>().ImplementedBy<BankTreeViewModel>());
            //container.Register(Component.For<IBankTreeViewItemViewModelFactory>().ImplementedBy<BankTreeViewItemViewModelFactory>());
            

            // Banks Accounts
            container.Register(Component.For<IWorkspace>().ImplementedBy<BankAccountsViewModel>());
            container.Register(Component.For<IBankAccountEditorViewModel>().ImplementedBy<BankAccountEditorViewModel>());

            // Transfers
            container.Register(Component.For<IWorkspace>().ImplementedBy<TransferListViewModel>());


            // Main
            container.Register(Component.For<IMainViewModel>().ImplementedBy<MainViewModel>());

        }
    }
}
