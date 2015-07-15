using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Finances.Core.Interfaces;

namespace Finances.Core.Engines.Cashflow
{
    public class TransferFrequencyDateCalculatorFactory : ITransferFrequencyDateCalculatorFactory
    {
        readonly IEnumerable<ITransferFrequencyDateCalculator> calculators;

        public TransferFrequencyDateCalculatorFactory(IEnumerable<ITransferFrequencyDateCalculator> calculators)
        {
            this.calculators = calculators;
        }


        public ITransferFrequencyDateCalculator Create(Entities.Transfer transfer)
        {
            var calculator = calculators.FirstOrDefault(c => c.Frequency == transfer.Frequency);

            if (calculators == null)
                throw new Exceptions.FinancesCoreException("TransferFrequencyDateCalculator not found for Transfer Frequency='{0}'", transfer.Frequency);

            return calculator;
        }
    }
}
