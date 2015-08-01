using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.Entities;
using Finances.Core.Interfaces;

namespace Finances.UnitTests.MS.Fakes
{
    class FakeBankAccountRepository : IBankAccountRepository
    {
        List<BankAccount> accounts;

        public FakeBankAccountRepository()
        {
            accounts = new List<BankAccount>()
            {
                new BankAccount()
                {
                    BankAccountId=1,
                    Name="Current"
                },
                new BankAccount()
                {
                    BankAccountId=2,
                    Name="Savings"
                }
            };

        }

        public int Add(Core.Entities.BankAccount account)
        {
            throw new NotImplementedException();
        }

        public bool Update(Core.Entities.BankAccount account)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Core.Entities.BankAccount account)
        {
            throw new NotImplementedException();
        }

        public bool Delete(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public Core.Entities.BankAccount Read(int accountId)
        {
            throw new NotImplementedException();
        }

        public List<Core.Entities.BankAccount> ReadList()
        {
            return accounts;
        }

        public List<Core.Entities.BankAccount> ReadListByBankId(int bankId)
        {
            throw new NotImplementedException();
        }

        public List<Core.Entities.DataIdName> ReadListDataIdName()
        {
            throw new NotImplementedException();
        }
    }
}
