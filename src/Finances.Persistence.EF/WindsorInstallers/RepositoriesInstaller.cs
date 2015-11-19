using Castle.Core;
using Castle.MicroKernel.Registration;

using Finances.Core.Interfaces;
using Finances.Persistence.EF.Interceptors;

namespace Finances.Persistence.EF.WindsorInstallers
{
    public class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Component.For<ErrorHandlingInterceptor>().LifestyleTransient());
            container.Register(Component.For<DbEntityValidationExceptionInterceptor>().LifestyleTransient());

            container.Register(Classes.FromThisAssembly()
                .BasedOn<IRepository>()
                .WithService.FromInterface()   // registers all class whose interfaces extended IService
                .Configure(cc => cc.Interceptors(InterceptorReference.ForType<ErrorHandlingInterceptor>()).AtIndex(0).Configuration()
                                   .Interceptors(InterceptorReference.ForType<DbEntityValidationExceptionInterceptor>()).AtIndex(1).Configuration()));
        }
    }
}
