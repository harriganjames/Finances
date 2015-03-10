using System;
using System.Collections.Generic;

namespace Finances.Core.Entities
{
    public class Cashflow
    {
        public Cashflow()
        {
            CashflowBankAccounts = new List<CashflowBankAccount>();
        }

        public int CashflowId { get; set; }
        public string Name { get; set; }
        public decimal OpeningBalance { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime RecordCreatedDateTime { get; set; }
        public DateTime RecordUpdatedDateTime { get; set; }

        public List<CashflowBankAccount> CashflowBankAccounts { get; set; }

        //public void AddCashflowBankAccount(BankAccount a)
        //{
        //    CashflowBankAccounts.Add(new CashflowBankAccount() { BankAccount = a });
        //}

    
    }
}
