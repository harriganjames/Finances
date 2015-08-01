using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Facilities.TypedFactory;

using Finances.WinClient.ViewModels;
using Finances.Core.Wpf;
using Finances.WinClient.Factories;
using Finances.Core.Interfaces;

namespace Finances.WinClient.CastleInstallers
{
    class ViewModelInstallers : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.AddFacility<TypedFactoryFacility>();

            IAppSettings appSettings = container.Resolve<IAppSettings>();

            bool debug;
            bool.TryParse(appSettings.GetSetting("debug"),out debug);

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

            // Banks Accounts
            container.Register(Component.For<IWorkspace>()
                .Forward<IBankAccountListViewModel>()
                .ImplementedBy<BankAccountListViewModel>()
                .LifeStyle.Transient
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
                .LifeStyle.Transient
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
                .LifeStyle.Transient
                );
            container.Register(Component.For<CashflowEditorViewModel>()
                .ImplementedBy<CashflowEditorViewModel>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<ICashflowEditorViewModelFactory>()
                .AsFactory()
                );



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



            // Cashflow Table
            container.Register(Component.For<IWorkspace>()
                .ImplementedBy<CashflowTableViewModel>()
                );


        }
    }
}
