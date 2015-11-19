using System.Collections.Generic;
using Finances.Core.Entities;

namespace Finances.Core.Interfaces
{
    public interface IBankAccountRepository : IRepository
    {
        int Add(BankAccount account);
        bool Update(BankAccount account);
        bool Delete(BankAccount account);
        bool Delete(List<int> ids);
        BankAccount Read(int accountId);
        List<BankAccount> ReadList();
        List<BankAccount> ReadListByBankId(int bankId);
        List<DataIdName> ReadListDataIdName();
    }
}
