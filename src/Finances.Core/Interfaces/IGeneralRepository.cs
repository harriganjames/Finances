using System.Collections.Generic;
using Finances.Core.Entities;

namespace Finances.Core.Interfaces
{
    public interface IGeneralRepository
    {
        List<Bank> GetDataTestProc();
        List<Bank> GetDataTestQuery();
        Bank GetBankById(int bankId);
        int InsertBankAndReadId(string name);
    }
}
