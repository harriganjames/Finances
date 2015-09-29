using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Finances.Core.Interfaces;

namespace Finances.Core.Entities
{
    public class Schedule //: IScheduleEntity
    {
        readonly IEnumerable<IScheduleFrequencyCalculator> calculators;
        IScheduleFrequencyCalculator calculator;

        public Schedule(IEnumerable<IScheduleFrequencyCalculator> calculators)
        {
            this.calculators = calculators;

            this.FrequencyEvery = 1;
        }

        public DateTime StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }

        string frequency;
        public string Frequency 
        {
            get
            {
                return frequency;
            }
            set
            {
                frequency = value;
                calculator = calculators.FirstOrDefault(c => c.Frequency == frequency);
            }
        }
        public int FrequencyEvery { get; set; }

        public string GetDescription()
        {
            var sb = new StringBuilder();

            if (StartDate == EndDate)
            {
                sb.AppendFormat("One-off on {0:yyyy-MM-dd}", StartDate);
            }
            else
            {
                sb.AppendFormat("Every {0} {1} from {2:yyyy-MM-dd}", FrequencyEvery, GetFrequencyEveryLabel(), StartDate);
                if (EndDate != null)
                {
                    sb.AppendFormat(" until {0:yyyy-MM-dd}", EndDate);
                }
            }

            return sb.ToString(); ;
        }

        public string GetFrequencyEveryLabel()
        {
            
            return Calculator.GetFrequencyEveryLabel(this.FrequencyEvery);
        }

        public DateTime CalculateNextDate(DateTime date)
        {
            return Calculator.CalculateNextDate(this, date);
        }


        private IScheduleFrequencyCalculator Calculator
        {
            get
            {
                if (calculator == null)
                    throw new Exceptions.FinancesCoreException("ScheduleFrequencyCalculator not available for Frequency='{0}'", Frequency);
                return this.calculator;
            }
        }


    }
}
