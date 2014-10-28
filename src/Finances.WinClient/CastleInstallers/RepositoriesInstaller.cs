using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Finances.Core.Interfaces;
using Finances.Persistence.FNH;
using Finances.WinClient.InterceptorSelectors;

namespace Finances.WinClient.CastleInstallers
{
    class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IBankRepository>().ImplementedBy<BankRepository>());
            //container.Register(Component.For<IBankMapper>().ImplementedBy<BankMapper>());

            container.Register(Component.For<IBankAccountRepository>().ImplementedBy<BankAccountRepository>());
            //container.Register(Component.For<IBankAccountMapper>().ImplementedBy<BankAccountMapper>());

            container.Register(Component.For<ITransferRepository>().ImplementedBy<TransferRepository>());

        }
    }
}
