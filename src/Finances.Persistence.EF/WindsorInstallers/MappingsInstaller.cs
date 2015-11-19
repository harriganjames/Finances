using Castle.Core;
using Castle.MicroKernel.Registration;

using Finances.Core.Interfaces;
using Finances.Interface;
using Finances.Persistence.EF.Interceptors;

namespace Finances.Persistence.EF.WindsorInstallers
{
    public class MappingsInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                .BasedOn<IMappingCreator>()
                .WithService.FromInterface()
                );
    
        }
    }
}
