using System;
using Finances.Core.Entities;

namespace Finances.Core.Interfaces
{
    public interface ITransferFrequencyDateCalculator
    {
        string Frequency { get; }
        DateTime CalculateNextDate(Transfer transfer, DateTime date);
    }
}
