using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Finances.Core.Interfaces;

namespace Finances.Core.Engines
{
    public class ScheduleFrequencyCalculatorFactory : IScheduleFrequencyCalculatorFactory
    {
        readonly IEnumerable<IScheduleFrequencyCalculator> calculators;

        public ScheduleFrequencyCalculatorFactory(IEnumerable<IScheduleFrequencyCalculator> calculators)
        {
            this.calculators = calculators;
        }


        public IScheduleFrequencyCalculator GetCalculator(Entities.Schedule schedule)
        {
            var calculator = calculators.FirstOrDefault(c => c.Frequency == schedule.Frequency);

            if (calculators == null)
                throw new Exceptions.FinancesCoreException("ScheduleFrequencyCalculator not found for Frequency='{0}'", schedule.Frequency);

            return calculator;
        }
    }
}
