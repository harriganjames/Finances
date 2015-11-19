using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Finances.Core.Interfaces;
using Finances.Core.Entities;

namespace Finances.Core.Engines
{
    public class ScheduleFrequencyMonthly : IScheduleFrequency
    {
        public string Frequency
        {
            get { return "Monthly"; }
        }

        public string GetFrequencyEveryLabel(int every)
        {
            return String.Format("month{0}", every == 1 ? String.Empty : "s");
        }

        public DateTime CalculateNextDate(Schedule schedule, DateTime date)
        {
            return date.AddMonths(schedule.FrequencyEvery);
        }

    }
}
