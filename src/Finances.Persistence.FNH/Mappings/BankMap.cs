using FluentNHibernate.Mapping;
using Finances.Core.Entities;

namespace Finances.Persistence.FNH.Mappings
{
    public class BankMap : ClassMap<Bank>
    {
        public BankMap()
        {
            Id(x => x.BankId);
            Map(x => x.Name);
            Map(x => x.Logo)
                 .Length(0x7fffffff);
       }
    }
}
