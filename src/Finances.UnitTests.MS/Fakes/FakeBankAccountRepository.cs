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

        public int Add(BankAccount account)
        {
            throw new NotImplementedException();
        }

        public bool Update(BankAccount account)
        {
            throw new NotImplementedException();
        }

        public bool Delete(BankAccount account)
        {
            throw new NotImplementedException();
        }

        public bool Delete(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public BankAccount Read(int accountId)
        {
            throw new NotImplementedException();
        }

        public List<BankAccount> ReadList()
        {
            return accounts;
        }

        public List<BankAccount> ReadListByBankId(int bankId)
        {
            throw new NotImplementedException();
        }

        public List<DataIdName> ReadListDataIdName()
        {
            throw new NotImplementedException();
        }
    }
}
