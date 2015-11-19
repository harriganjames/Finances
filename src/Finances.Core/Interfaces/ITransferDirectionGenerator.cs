using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using Finances.Core.Engines.Cashflow;
using Finances.Core.Entities;
using Finances.Core.ValueObjects;

namespace Finances.Core.Interfaces
{
    public interface ITransferDirectionGenerator
    {
        List<TransferDirection> GetTransferDirections(List<BankAccount> bankAccounts);
    }
}
