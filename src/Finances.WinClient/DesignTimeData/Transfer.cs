using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.WinClient.DesignTimeData
{
    public class Transfer
    {
        public string Name { get; set; }

        public List<BankAccount> BankAccounts { get; }

        public BankAccount FromBankAccount { get; }

        public BankAccount ToBankAccount { get; }

    }
}
