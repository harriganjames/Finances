using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;

namespace Finances.WinClient.CastleInstallers
{
    public class InterceptorsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<Finances.WinClient.Interceptors.ErrorHandlingInterceptor>());
        }
    }
}
