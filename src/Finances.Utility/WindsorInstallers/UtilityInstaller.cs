using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;


namespace Finances.Utility.WindsorInstallers
{
    public class UtilityInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component
                        .For(typeof(IDebounceExecutor<>))
                        .ImplementedBy(typeof(DebounceExecutor<>))
                        .LifestyleTransient());

        }
    }
}
