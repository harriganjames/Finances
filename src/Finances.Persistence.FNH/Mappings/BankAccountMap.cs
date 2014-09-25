using System;
using Finances.Core.Entities;
using FluentNHibernate.Mapping;

namespace Finances.Persistence.FNH.Mappings
{
    class BankAccountMap
    {
        class AccountMap : ClassMap<BankAccount>
        {
            public AccountMap()
            {
                Id(x => x.BankAccountId);
                Map(x => x.Name);
                //Map(x=>x.BankId);
                Map(x => x.SortCode);
                Map(x => x.AccountNumber);
                Map(x => x.AccountOwner);
                Map(x => x.PaysTaxableInterest);
                Map(x => x.LoginURL);
                Map(x => x.LoginID);
                Map(x => x.PasswordHint);
                Map(x => x.OpenedDate);
                Map(x => x.ClosedDate);
                Map(x => x.InitialRate);
                Map(x => x.MilestoneDate);
                Map(x => x.MilestoneNotes);
                Map(x => x.Notes);
                Map(x => x.RecordCreatedDateTime)
                    .Generated
                    .Always();
                Map(x => x.RecordUpdatedDateTime)
                    .Generated
                    .Always();

                //Map(x => x.Bank);

                References(x => x.Bank)
                    .Column("BankId")
                    .Cascade.None();

            }
        }
    }
}
