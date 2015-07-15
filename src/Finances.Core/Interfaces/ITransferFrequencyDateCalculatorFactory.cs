using System;
using Finances.Core.Entities;

namespace Finances.Core.Interfaces
{
    public interface ITransferFrequencyDateCalculatorFactory
    {
        ITransferFrequencyDateCalculator Create(Transfer trransfer);
    }
}
