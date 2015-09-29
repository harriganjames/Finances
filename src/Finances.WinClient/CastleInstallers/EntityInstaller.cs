using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Facilities.TypedFactory;

using Finances.Core.Factories;
using Finances.Core.Entities;

namespace Finances.WinClient.CastleInstallers
{
    public class EntityInstaller : IWindsorInstaller
    {

        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {

            container.Register(Component.For<Schedule>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<IScheduleFactory>()
                .AsFactory()
                );

            
            container.Register(Component.For<Transfer>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<ITransferFactory>()
                .AsFactory()
                );


            container.Register(Component.For<Cashflow>()
                .LifeStyle.Transient
                );
            container.Register(Component.For<ICashflowFactory>()
                .AsFactory()
                );



        }
    }
}
