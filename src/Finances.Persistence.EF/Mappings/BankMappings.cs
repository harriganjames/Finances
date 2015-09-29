using System;
using AutoMapper;
using Finances.Core.Interfaces;

namespace Finances.Persistence.EF.Mappings
{
    public class BankMappings : IMappingCreator
    {

        public void CreateMappings()
        {
            Mapper.CreateMap<Bank, Core.Entities.Bank>().ReverseMap();

            Mapper.CreateMap<Bank, Core.Entities.DataIdName>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.BankId))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));
        }
    }
}
