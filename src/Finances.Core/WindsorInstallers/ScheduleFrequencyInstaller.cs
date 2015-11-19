using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Registration;
using Finances.Core.Interfaces;
using Finances.Core.Engines;
using Finances.Core.Factories;

namespace Finances.Core.WindsorInstallers
{
    public class ScheduleFrequencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IScheduleFrequency>()
                .ImplementedBy<ScheduleFrequencyMonthly>());
            container.Register(Component.For<IScheduleFrequency>()
                .ImplementedBy<ScheduleFrequencyWeekly>());

            container.Register(Component.For<IScheduleFrequencyFactory>()
                .ImplementedBy<ScheduleFrequencyFactory>());
        }
    }
}
