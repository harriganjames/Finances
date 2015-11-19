using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Core;
using Finances.WinClient.ViewModels;

namespace Finances.WinClient.CastleInstallers
{
    public class WorkspaceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //container.Register(Component.For<Finances.Core.Wpf.IWorkspaceCollection>().ImplementedBy<Finances.Core.Wpf.WorkspaceCollection>());

            //container.Register(Component.For<Finances.Core.Wpf.IWorkspaceManager>()
            //                        .Properties(PropertyFilter.IgnoreAll)   // disable property injection
            //                        .ImplementedBy<Finances.Core.Wpf.WorkspaceManager>());

            container.Register(Component.For<IWorkspaceAreaViewModel>()
                                    .Properties(PropertyFilter.IgnoreAll)   // disable property injection
                                    .ImplementedBy<WorkspaceAreaViewModel>());
        }
    }
}
