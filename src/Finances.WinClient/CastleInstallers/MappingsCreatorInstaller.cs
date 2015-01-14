using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Finances.Core;
using AutoMapper;

namespace Finances.WinClient.CastleInstallers
{
    class MappingsCreatorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // this is only used to create the mappings. the instance isn't used anywhere
            container.Register(Component.For<MappingsCreator>().ImplementedBy<MappingsCreator>());

            container.Register(Component.For<IMappingEngine>().UsingFactoryMethod(() =>
                {
                    return Mapper.Engine;
                }));
        }
    }
}
