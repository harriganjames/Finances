using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Finances.Core;
using Finances.Persistence.EF;

namespace Finances.WinClient.CastleInstallers
{
    public class EFModelContextFactoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IModelContextFactory>().UsingFactoryMethod(() =>
                {
                    return new ModelContextFactory(container.Resolve<IConnection>().ConnectionString);
                }
                ));

        }
    }
}
