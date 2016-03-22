using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Finances.Core.Wpf;
using Finances.WinClient.ViewModels;

namespace Finances.WinClient.WindsorInstallers
{
    public class WorkspacesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IWorkspace>()
                .Forward<BankListViewModel>()
                .ImplementedBy<BankListViewModel>()
                .LifeStyle.Transient
                );

            container.Register(Component.For<IWorkspace>()
                .Forward<BankAccountListViewModel>()
                .ImplementedBy<BankAccountListViewModel>()
                .LifeStyle.Transient
                );

            container.Register(Component.For<IWorkspace>()
                .Forward<TransferListViewModel>() // for dashboard
                .ImplementedBy<TransferListViewModel>()
                .LifeStyle.Transient
                );

            container.Register(Component.For<IWorkspace>()
                .Forward<CashflowListViewModel>() // for dashboard
                .ImplementedBy<CashflowListViewModel>()
                .LifeStyle.Transient
                );

            container.Register(Component.For<IWorkspace>()
                .ImplementedBy<CashflowTableViewModel>()
                );

        }

    }
}
