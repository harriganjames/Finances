using System;
using Finances.Core.Entities;

namespace Finances.Core.Interfaces
{
    public interface IScheduleFrequencyCalculator
    {
        string Frequency { get; }
        string GetFrequencyEveryLabel(int every);
        DateTime CalculateNextDate(Schedule schedule, DateTime date);
    }
}
