using System;
using Finances.Core.Entities;
using Finances.Core.Interfaces;

namespace Finances.Core.Factories
{
    public interface IScheduleFrequencyFactory
    {
        IScheduleFrequency GetFrequency(Schedule schedule);
    }
}
