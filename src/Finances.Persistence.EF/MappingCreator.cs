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
                //.ForMember(a => a.Bank, opt => opt.Ignore())
                .ForMember(a => a.BankId, opt => opt.MapFrom(e => e.Bank.BankId));
        
            Mapper.CreateMap<BankAccount, Core.Entities.DataIdName>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.BankAccountId))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => String.Format("{0} - {1}", s.Bank.Name, s.Name)));


            // Transfers
            Mapper.CreateMap<Transfer, Core.Entities.Transfer>().ReverseMap();

            Mapper.CreateMap<Transfer, Core.Entities.DataIdName>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.TransferId))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));


        }
    }

}
