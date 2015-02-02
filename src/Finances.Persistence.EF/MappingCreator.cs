using System;
using AutoMapper;
using Finances.Core.Interfaces;

namespace Finances.Persistence.EF
{
    public class MappingCreator : IMappingCreator
    {
        public void CreateMappings()
        {
            //Banks
            Mapper.CreateMap<Bank, Core.Entities.Bank>().ReverseMap();

            Mapper.CreateMap<Bank, Core.Entities.DataIdName>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.BankId))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));

        
            // Bank Accounts
            Mapper.CreateMap<BankAccount, Core.Entities.BankAccount>();
            Mapper.CreateMap<Core.Entities.BankAccount, BankAccount>()
                .ForMember(a => a.BankId, opt => opt.MapFrom(e => e.Bank.BankId))
                .ForMember(a => a.Bank, opt => opt.Ignore());
        
            Mapper.CreateMap<BankAccount, Core.Entities.DataIdName>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.BankAccountId))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => String.Format("{0} ({1})", s.Name, s.Bank.Name)));


            // Transfers
            Mapper.CreateMap<Transfer, Core.Entities.Transfer>();
            Mapper.CreateMap<Core.Entities.Transfer, Transfer>()
                .ForMember(t => t.FromBankAccountId, opt => opt.MapFrom(s => s.FromBankAccount.BankAccountId))
                .ForMember(t => t.ToBankAccountId, opt => opt.MapFrom(s => s.ToBankAccount.BankAccountId))
                .ForMember(t => t.FromBankAccount, opt => opt.Ignore())
                .ForMember(t => t.ToBankAccount, opt => opt.Ignore());

            Mapper.CreateMap<Transfer, Core.Entities.DataIdName>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.TransferId))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));


        }
    }

}
