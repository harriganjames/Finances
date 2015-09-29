using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Finances.WinClient.DomainServices;
using Finances.WinClient.ViewModels;
using Finances.WinClient.InterceptorSelectors;
using Finances.Core.Interfaces;

namespace Finances.WinClient.CastleInstallers
{
    public class DomainServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IBankAgent>().ImplementedBy<BankAgent>());

            container.Register(Component.For<IBankAccountAgent>().ImplementedBy<BankAccountAgent>());

            container.Register(Component.For<ITransferAgent>().ImplementedBy<TransferAgent>());

            container.Register(Component.For<ICashflowAgent>().ImplementedBy<CashflowAgent>());


            //container.Kernel.ProxyFactory.AddInterceptorSelector(new DomainServiceInterceptorSelector());

        }
    }
}
