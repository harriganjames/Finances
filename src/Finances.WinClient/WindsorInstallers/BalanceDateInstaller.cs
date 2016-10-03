using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Facilities.TypedFactory;

using Finances.WinClient.ViewModels;
using Finances.Core.Wpf;
using Finances.WinClient.Factories;
using Finances.Core.Interfaces;
using Finances.WinClient.DomainServices;

namespace Finances.WinClient.WindsorInstallers
{
    public class BalanceDateInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            //IAppSettings appSettings = container.Resolve<IAppSettings>();

            //bool debug;
            //bool.TryParse(appSettings.GetSetting("debug"),out debug);

            container.Register(Component.For<BalanceDateEditorViewModel>()
                .ImplementedBy<BalanceDateEditorViewModel>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<IBalanceDateEditorViewModelFactory>()
                .AsFactory()
                );


            container.Register(Component.For<IBalanceDateAgent>()
                .ImplementedBy<BalanceDateAgent>());

        }
    }
}
