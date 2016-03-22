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

        public List<BankAccount> BankAccounts { get; set;  }

        public BankAccount FromBankAccount { get; set;  }

        public BankAccount ToBankAccount { get; set;  }

    }
}
