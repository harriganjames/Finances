using Castle.Core;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;

//using Finances.Core.Interfaces;

using Finances.Persistence.EF.Interceptors;

namespace Finances.Persistence.EF.WindsorInstallers
{
    public class InterceptorsInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            //container.Register(Component.For<ErrorHandlingInterceptor>().LifestyleTransient());
            //container.Register(Component.For<DbEntityValidationExceptionInterceptor>().LifestyleTransient());

            container.Register(Classes.FromThisAssembly()
                    .BasedOn<IInterceptor>()
                    //.WithService.FromInterface()
                    .LifestyleTransient()
                    );
        }
    }
}
