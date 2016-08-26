using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.WinClient.DesignTimeData
{
    public class BalanceDate
    {
        public BalanceDate()
        {
            BalanceDateBankAccounts = new List<BalanceDateBankAccount>();
            BalanceDateBankAccounts.Add(new BalanceDateBankAccount());
            BalanceDateBankAccounts.Add(new BalanceDateBankAccount() { BalanceAmount=345 });
            DateOfBalance = new DateTime(2016, 08, 25);
        }
        public DateTime DateOfBalance { get; set; }
        public List<BalanceDateBankAccount> BalanceDateBankAccounts { get; set; }
    }
}
