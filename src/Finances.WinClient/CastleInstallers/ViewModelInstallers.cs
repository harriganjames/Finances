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
                .ImplementedBy<BanksViewModel>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<IBankEditorViewModel>()
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
                .ImplementedBy<BankAccountsViewModel>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<IBankAccountEditorViewModel>()
                .ImplementedBy<BankAccountEditorViewModel>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<IBankAccountEditorViewModelFactory>()
                .AsFactory()
                );



            // Transfers
            container.Register(Component.For<IWorkspace>()
                .ImplementedBy<TransferListViewModel>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<ITransferEditorViewModel>()
                .ImplementedBy<TransferEditorViewModel>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<ITransferEditorViewModelFactory>()
                .AsFactory()
                );


            // Main
            container.Register(Component.For<IMainViewModel>().ImplementedBy<MainViewModel>());

        }
    }
}
