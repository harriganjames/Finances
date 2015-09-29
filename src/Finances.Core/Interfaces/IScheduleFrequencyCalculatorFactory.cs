using System;
using Finances.Core.Entities;

namespace Finances.Core.Interfaces
{
    public interface IScheduleFrequencyCalculatorFactory
    {
        IScheduleFrequencyCalculator GetCalculator(Schedule schedule);
    }
}
