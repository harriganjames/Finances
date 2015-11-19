using Castle.MicroKernel.Registration;
using Finances.Core.Interfaces;
using Finances.Interface;
using Finances.Persistence.EF.Mappings;

namespace Finances.Persistence.EF.WindsorInstallers
{
    public class CashflowInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Component
                .For<IRepositoryWrite<Finances.Core.Entities.Cashflow>>()
                .ImplementedBy<Finances.Persistence.EF.RepositoryWriteDecoratorTest<Finances.Core.Entities.Cashflow>>());

            container.Register(Component
                .For<IRepositoryWrite<Finances.Core.Entities.Cashflow>>()
                .ImplementedBy<Finances.Persistence.EF.RepositoryWriteExceptionHandlerDecorator<Finances.Core.Entities.Cashflow>>());

            container.Register(Component
                .For<IRepositoryWrite<Finances.Core.Entities.Cashflow>>()
                .ImplementedBy<Finances.Persistence.EF.RepositoryWriteEntityValidationHandler<Finances.Core.Entities.Cashflow>>());

            container.Register(Component
                .For<IRepositoryRead<Finances.Core.Entities.Cashflow>>()
                .Forward<IRepositoryWrite<Finances.Core.Entities.Cashflow>>()
                .ImplementedBy<Finances.Persistence.EF.CashflowRepository>());

            //container.Register(Component.For<IMappingCreator>().ImplementedBy<CashflowMappings>());
        }
    }
}
