using Castle.MicroKernel.Registration;
using Finances.Core.Interfaces;
using Finances.Persistence.EF.Interfaces;

namespace Finances.Persistence.EF.WindsorInstallers
{
    public class EntityValidationInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Component
                .For<IEntityValidationHandler>()
                .ImplementedBy<Finances.Persistence.EF.EntityValidationHandler>());
        }
    }
}
