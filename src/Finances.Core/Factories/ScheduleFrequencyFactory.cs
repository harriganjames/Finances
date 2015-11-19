using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Finances.Core.Interfaces;

namespace Finances.Core.Factories
{
    public class ScheduleFrequencyFactory : IScheduleFrequencyFactory
    {
        readonly IEnumerable<IScheduleFrequency> calculators;

        public ScheduleFrequencyFactory(IEnumerable<IScheduleFrequency> calculators)
        {
            this.calculators = calculators;
        }


        public IScheduleFrequency GetFrequency(Entities.Schedule schedule)
        {
            var calculator = calculators.FirstOrDefault(c => c.Frequency == schedule.Frequency);

            if (calculators == null)
                throw new Exceptions.FinancesCoreException("ScheduleFrequencyCalculator not found for Frequency='{0}'", schedule.Frequency);

            return calculator;
        }
    }
}
