using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Core;

namespace Finances.Core.Wpf.WindsorInstallers
{
    public class WorkspaceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IWorkspaceCollection>()
                                    .ImplementedBy<WorkspaceCollection>());

            container.Register(Component.For<IWorkspaceManager>()
                                    .Properties(PropertyFilter.IgnoreAll)   // disable property injection
                                    .ImplementedBy<WorkspaceManager>());
        }
    }
}
