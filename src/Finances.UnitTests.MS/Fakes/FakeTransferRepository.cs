using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.Entities;
using Finances.Core.Factories;
using Finances.Core.Interfaces;

namespace Finances.UnitTests.MS.Fakes
{
    public class FakeTransferRepository : ITransferRepository
    {
        List<Transfer> transfers;

        public FakeTransferRepository()
        {
            IScheduleFactory scheduleFactory = new FakeScheduleFactory();

            transfers = new List<Transfer>()
            {
                new Transfer(scheduleFactory)
                {
                    TransferId=1,
                    FromBankAccount = new BankAccount() { BankAccountId=1 },
                    ToBankAccount = new BankAccount() { BankAccountId=2 },
                    Amount = 100,
                    IsEnabled = true
                }
            };
        }


        public int Add(Transfer data)
        {
            throw new NotImplementedException();
        }

        public bool Update(Transfer data)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Transfer data)
        {
            throw new NotImplementedException();
        }

        public bool Delete(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public Transfer Read(int dataId)
        {
            throw new NotImplementedException();
        }

        public List<Transfer> ReadList()
        {
            return transfers;
        }

        public List<DataIdName> ReadListDataIdName()
        {
            throw new NotImplementedException();
        }

        public List<TransferCategory> ReadListTransferCategories()
        {
            throw new NotImplementedException();
        }
    }
}
