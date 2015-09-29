using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.Entities;
using Finances.Core.Factories;
using Finances.Core.Engines;
using Finances.Core.Interfaces;
//using Finances.WinClient.Factories;

namespace Finances.UnitTests.MS.Fakes
{
    public class FakeScheduleFactory : IScheduleFactory
    {
        public Schedule Create()
        {
            return new Schedule(new IScheduleFrequencyCalculator[] { 
                                new ScheduleFrequencyCalculatorMonthly(),
                                new ScheduleFrequencyCalculatorWeekly() });
        }

        public void Release(Schedule s)
        {
        }
    }
}
