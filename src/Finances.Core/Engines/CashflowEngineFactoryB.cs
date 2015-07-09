using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.Engines.Cashflow;
using Finances.Core.Interfaces;

namespace Finances.Core.Engines
{
    public class CashflowEngineFactoryB : ICashflowEngineFactoryB
    {
        readonly IBankAccountRepository bankAccountRepository;
        readonly ITransferRepository transferRepository;

        public CashflowEngineFactoryB(
                        IBankAccountRepository bankAccountRepository,
                        ITransferRepository transferRepository
                        )
        {
            this.bankAccountRepository = bankAccountRepository;
            this.transferRepository = transferRepository;                
        }


        public ICashflowEngineB CreateDetail()
        {
            ICashflowEngineB e = new CashflowEngineB(this.bankAccountRepository, this.transferRepository,new DetailAggregatedProjectionItemsGenerator());
            return e;
        }

        public ICashflowEngineB CreateMonthlySummary()
        {
            ICashflowEngineB e = new CashflowEngineB(this.bankAccountRepository, this.transferRepository, new MonthlySummaryAggregatedProjectionItemsGenerator());
            return e;
        }

    }
}
