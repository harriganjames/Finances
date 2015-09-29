using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Finances.Core.Factories;
using Finances.Core.Entities;

namespace Finances.IntegrationTests.MS.Fakes
{
    public class FakeTransferFactory : ITransferFactory
    {
        readonly IScheduleFactory scheduleFactory;

        public FakeTransferFactory(IScheduleFactory scheduleFactory)
        {
            this.scheduleFactory = scheduleFactory;
        }

        public Transfer Create()
        {
            return new Transfer(this.scheduleFactory);
        }

        public void Release(Transfer s)
        {
        }
    }
}
