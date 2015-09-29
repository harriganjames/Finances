using System;
using Finances.Core.Entities;

namespace Finances.Core.Factories
{
    public interface IScheduleFactory
    {
        Schedule Create();
        void Release(Schedule s);
    }

}
