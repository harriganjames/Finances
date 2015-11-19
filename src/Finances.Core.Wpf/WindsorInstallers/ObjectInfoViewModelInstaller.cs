using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Finances.Core.Wpf;
using Finances.Core.Wpf.ObjectInfo;
using System.Windows;
using System.Windows.Threading;
using Finances.Core;
using Finances.Service;

namespace Finances.Core.Wpf.WindsorInstallers
{
    public class ObjectInfoViewModelInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ObjectInfoViewModel>().ImplementedBy<ObjectInfoViewModel>());
        }
    }
}
