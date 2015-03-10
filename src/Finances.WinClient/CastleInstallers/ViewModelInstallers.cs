using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Facilities.TypedFactory;

using Finances.WinClient.ViewModels;
using Finances.Core.Wpf;
using Finances.WinClient.Factories;

namespace Finances.WinClient.CastleInstallers
{
    class ViewModelInstallers : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.AddFacility<TypedFactoryFacility>();

            
            // Banks
            container.Register(Component.For<IWorkspace>()
                .Forward<IBanksViewModel>()
                .ImplementedBy<BanksViewModel>()
                );
            container.Register(Component.For<BankEditorViewModel>()
                .ImplementedBy<BankEditorViewModel>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<IBankEditorViewModelFactory>()
                .AsFactory()
                );
            

            // Bank Tree
            container.Register(Component.For<IWorkspace>().ImplementedBy<BankTreeViewModel>());
            

            // Banks Accounts
            container.Register(Component.For<IWorkspace>()
                .Forward<IBankAccountsViewModel>()
                .ImplementedBy<BankAccountsViewModel>()
                );
            container.Register(Component.For<BankAccountEditorViewModel>()
                .ImplementedBy<BankAccountEditorViewModel>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<IBankAccountEditorViewModelFactory>()
                .AsFactory()
                );



            // Transfers
            container.Register(Component.For<IWorkspace>()
                .Forward<ITransferListViewModel>() // for dashboard
                .ImplementedBy<TransferListViewModel>()
                );
            container.Register(Component.For<TransferEditorViewModel>()
                .ImplementedBy<TransferEditorViewModel>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<ITransferEditorViewModelFactory>()
                .AsFactory()
                );


            // Cashflows
            container.Register(Component.For<IWorkspace>()
                .Forward<ICashflowListViewModel>() // for dashboard
                .ImplementedBy<CashflowListViewModel>()
                );
            container.Register(Component.For<CashflowEditorViewModel>()
                .ImplementedBy<CashflowEditorViewModel>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<ICashflowEditorViewModelFactory>()
                .AsFactory()
                );



            // Dashboard
            container.Register(Component.For<IWorkspace>()
                .ImplementedBy<DashboardViewModel>()
                );


            // Main
            container.Register(Component.For<IMainViewModel>().ImplementedBy<MainViewModel>());

        }
    }
}
