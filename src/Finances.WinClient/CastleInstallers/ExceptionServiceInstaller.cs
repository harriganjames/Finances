using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Finances.Core.Wpf;

namespace Finances.WinClient.CastleInstallers
{
    public class ExceptionServiceInstaller :IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IExceptionService>().ImplementedBy<ExceptionService>());
        }
    }
}
