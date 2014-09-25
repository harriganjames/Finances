using System.Collections.Generic;
using Finances.Core.Entities;

namespace Finances.Core.Interfaces
{
    public interface IBankAccountRepository
    {
        int Add(BankAccount account);
        bool Update(BankAccount account);
        //bool Delete(BankAccount account);
        bool Delete(int accountId);
        BankAccount Read(int accountId);
        List<BankAccount> ReadList();
        List<DataIdName> ReadListDataIdName();
    }
}
