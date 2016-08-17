using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.Entities
{
    public class BalanceDate
    {
        public BalanceDate()
        {
            BalanceDateBankAccounts = new List<BalanceDateBankAccount>();
        }
        public int BalanceDateId { get; set; }
        public DateTime DateOfBalance { get; set; }
        public List<BalanceDateBankAccount> BalanceDateBankAccounts { get; set; }

        public DateTime RecordCreatedDateTime { get; set; }
    }
}
