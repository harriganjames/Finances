using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Finances.Core.Interfaces;
using Finances.Core.Entities;

namespace Finances.Core.Engines
{
    public class ScheduleFrequencyWeekly : IScheduleFrequency
    {
        public string Frequency
        {
            get { return "Weekly"; }
        }

        public string GetFrequencyEveryLabel(int every)
        {
            return String.Format("week{0}", every == 1 ? String.Empty : "s");
        }

        public DateTime CalculateNextDate(Schedule schedule, DateTime date)
        {
            return date.AddDays(schedule.FrequencyEvery*7);
        }

    }
}
