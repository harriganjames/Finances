using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.WinClient.DesignTimeData
{
    public class BalanceDate
    {
        int nextBankAccount = 0;
        public BalanceDate()
        {
            BalanceDateBankAccounts = new List<BalanceDateBankAccount>();
            //BalanceDateBankAccounts.Add(new BalanceDateBankAccount());
            //BalanceDateBankAccounts.Add(new BalanceDateBankAccount() { BalanceAmount=345 });
            //DateOfBalance = new DateTime(2016, 08, 25);
        }
        public DateTime DateOfBalance { get; set; }
        public List<BalanceDateBankAccount> BalanceDateBankAccounts { get; set; }

        public BalanceDateBankAccount NextBalanceDateBankAccount
        {
            get
            {
                BalanceDateBankAccount r;
                if (BalanceDateBankAccounts.Count == 0) return null;
                r = BalanceDateBankAccounts[nextBankAccount];
                if (++nextBankAccount >= BalanceDateBankAccounts.Count) nextBankAccount = 0;
                return r;
            }
        }

    }
}
