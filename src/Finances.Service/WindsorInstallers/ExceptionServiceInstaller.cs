using Castle.MicroKernel.Registration;
using Finances.Core.Interfaces;
using Finances.Interface;

namespace Finances.Service.WindsorInstallers
{
    public class ExceptionServiceInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Component.For<IExceptionService>().ImplementedBy<ExceptionService>());
        }
    }
}
