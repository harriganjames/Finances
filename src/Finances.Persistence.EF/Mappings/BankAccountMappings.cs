using System;
using AutoMapper;
using Finances.Core.Interfaces;

namespace Finances.Persistence.EF.Mappings
{
    public class BankAccountMappings : IMappingCreator
    {
        public void CreateMappings()
        {
            Mapper.CreateMap<BankAccount, Core.Entities.BankAccount>();
            Mapper.CreateMap<Core.Entities.BankAccount, BankAccount>()
                .ForMember(a => a.BankId, opt => opt.MapFrom(e => e.Bank.BankId))
                .ForMember(a => a.Bank, opt => opt.Ignore());

            Mapper.CreateMap<BankAccount, Core.Entities.DataIdName>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.BankAccountId))
                //.ForMember(d => d.Name, opt => opt.MapFrom(s => String.Format("{0} ({1})", s.Name, s.Bank.Name)));
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name + " (" + s.Bank.Name + ")"));

        }
    }
}
