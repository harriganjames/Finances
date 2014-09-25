using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Finances.Core.Interfaces;
using Finances.Persistence.FNH;
using Finances.WinClient.Mappers;

namespace Finances.IntegrationTests.CastleInstallers
{
    class BankRepositoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IBankRepository>().ImplementedBy<BankRepository>());

            container.Register(Component.For<IBankMapper>().ImplementedBy<BankMapper>());

        }
    }
}
