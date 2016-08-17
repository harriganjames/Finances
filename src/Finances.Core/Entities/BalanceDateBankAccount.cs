using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.Entities
{
    public class BalanceDateBankAccount
    {
        public int BalanceDateBankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }
        public decimal BalanceAmount { get; set; }

        public DateTime RecordCreatedDateTime { get; set; }
    }
}
