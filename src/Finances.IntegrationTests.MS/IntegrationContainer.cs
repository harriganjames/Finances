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
using Castle.Windsor.Installer;
using Castle.MicroKernel.Registration;
using System.IO;
using System.Reflection;
using Finances.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Finances.Service.WindsorInstallers;
using Finances.Persistence.EF;
using Finances.Core;
using Finances.Interface;

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

            //selectively install Finance.Service classes
            container.Install(  new MappingsCreatorInstaller()
                                //new MappingsInstaller()
                                );

            //Persistence.EF installers
            container.Install(FromAssembly.Containing<ModelContextFactory>());

            //Core installers
            container.Install(FromAssembly.Containing<AppSettings>());

            //Integration inplememntations
            container.Register(Component.For<IConnection>().ImplementedBy<IntegrationConnection>());
            container.Register(Component.For<IExceptionService>().ImplementedBy<IntegrationExceptionService>());

            //Create all mappings
            container.Resolve<MappingsCreator>();

            DisplayRegistrations();


        }

        public WindsorContainer Container
        {
            get
            {
                return container;
            }
        }

        public void DisplayRegistrations()
        {
            foreach (var handler in container.Kernel.GetAssignableHandlers(typeof(object)))
            {
                Debug.Write(String.Format("{0} :", handler.ComponentModel.Implementation));
                foreach (var service in handler.ComponentModel.Services)
                {
                    Debug.Write(String.Format(" {0}", service.Name));
                }
                Debug.WriteLine("");
            }
        }


    }
}
