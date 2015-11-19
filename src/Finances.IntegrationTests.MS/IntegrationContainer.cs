using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.ModelBuilder.Inspectors;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Finances.Persistence.EF.WindsorInstallers;
using Finances.WinClient.CastleInstallers;

namespace Finances.IntegrationTests.MS
{
    public class IntegrationContainer
    {
        WindsorContainer container;


        public IntegrationContainer()
        {
            container = new WindsorContainer();

            // allow collection injection
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            // disable automatic property injection
            container.Kernel.ComponentModelBuilder.RemoveContributor(
                container.Kernel.ComponentModelBuilder.Contributors.OfType<PropertiesDependenciesModelInspector>().Single());

            
            
            container.Kernel.AddFacility<TypedFactoryFacility>();

            container.Install(
                            //new SystemInstaller(),
                            new ConnectionInstaller(),
                            new EFModelContextFactoryInstaller(),
                            //new DialogServiceInstaller(w),
                            //new RepositoriesInstaller(),
                            //new BankInstaller(),
                            //new BankAccountInstaller(),
                            //new TransferInstaller(),
                            new CashflowInstaller()
                            //new DomainServicesInstaller(),
                            //new ExceptionServiceInstaller(),
                            //new InterceptorsInstaller(),
                            //new WorkspaceInstaller(),
                            //new ViewModelInstallers(),
                            //new UtilitiesInstaller(w),
                            //new MappingsCreatorInstaller()
                            //new EnginesInstaller(),
                            //new EntityInstaller()
                            );


        }

        public WindsorContainer Container
        {
            get
            {
                return container;
            }
        }

    }
}
