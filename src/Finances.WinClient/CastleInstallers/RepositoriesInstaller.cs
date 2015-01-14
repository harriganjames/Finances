using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Finances.Core.Interfaces;
using Finances.Persistence.FNH;
using Finances.WinClient.InterceptorSelectors;
using Finances.Persistence.EF;

namespace Finances.WinClient.CastleInstallers
{
    class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IBankRepository>().ImplementedBy<Finances.Persistence.EF.BankRepository>());

            container.Register(Component.For<IBankAccountRepository>().ImplementedBy<Finances.Persistence.EF.BankAccountRepository>());

            container.Register(Component.For<ITransferRepository>().ImplementedBy<Finances.Persistence.EF.TransferRepository>());

            // Mappings
            container.Register(Component.For<IMappingCreator>().ImplementedBy<MappingCreator>());
        }
    }
}
