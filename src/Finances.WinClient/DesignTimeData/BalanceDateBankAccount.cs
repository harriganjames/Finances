using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.WinClient.DesignTimeData
{
    public class BalanceDateBankAccount
    {
        public BalanceDateBankAccount()
        {
            BankAccount = new BankAccount();
            BalanceAmount = 123;
        }
        public BankAccount BankAccount { get; set; }
        public decimal BalanceAmount { get; set; }
    }
}
