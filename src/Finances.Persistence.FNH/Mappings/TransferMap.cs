using System;
using Finances.Core.Entities;
using FluentNHibernate.Mapping;

namespace Finances.Persistence.FNH.Mappings
{
    class TransferMap
    {
        class AccountMap : ClassMap<Transfer>
        {
            public AccountMap()
            {
                Id(x => x.TransferId);
                Map(x => x.Name);
                Map(x => x.Amount);
                Map(x => x.AmountTolerence);
                Map(x => x.StartDate);
                Map(x => x.EndDate);
                Map(x => x.Frequency);
                Map(x => x.IsEnabled);
                Map(x => x.RecordCreatedDateTime)
                    .Generated
                    .Always();
                Map(x => x.RecordUpdatedDateTime)
                    .Generated
                    .Always();

                References(x => x.FromBankAccount)
                    .Column("FromBankAccountId")
                    .Cascade.None();

                References(x => x.ToBankAccount)
                    .Column("ToBankAccountId")
                    .Cascade.None();

            }
        }
    }
}
