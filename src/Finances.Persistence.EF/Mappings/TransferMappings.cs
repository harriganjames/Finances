using System;
using AutoMapper;
using Finances.Core.Factories;
using Finances.Core.Interfaces;

namespace Finances.Persistence.EF.Mappings
{
    public class TransferMappings : IMappingCreator
    {
        readonly ITransferFactory transferFactory;

        public TransferMappings(ITransferFactory transferFactory)
        {
            this.transferFactory = transferFactory;
        }

        public void CreateMappings()
        {
            //EF.Transfer.ScheduleStartDate => Core.Transfer.Schedule.StartDate

            Mapper.Configuration.RecognizePrefixes("Schedule");

            Mapper.CreateMap<Transfer, Core.Entities.Schedule>();

            Mapper.CreateMap<Transfer, Core.Entities.Transfer>()
                .ForMember(t => t.Category, opt => opt.MapFrom(s => s.TransferCategory))
                .ForMember(t => t.Schedule, opt => opt.MapFrom(s => s))
                .ConstructUsing((Transfer t) => this.transferFactory.Create());

            Mapper.CreateMap<Core.Entities.Transfer, Transfer>()
                .ForMember(t => t.FromBankAccountId, opt => opt.MapFrom(s => s.FromBankAccount.BankAccountId))
                .ForMember(t => t.ToBankAccountId, opt => opt.MapFrom(s => s.ToBankAccount.BankAccountId))
                .ForMember(t => t.FromBankAccount, opt => opt.Ignore())
                .ForMember(t => t.ToBankAccount, opt => opt.Ignore())
                .ForMember(t => t.TransferCategoryId, opt => opt.MapFrom(s => s.Category.TransferCategoryId))
                .ForMember(t => t.TransferCategory, opt => opt.Ignore())
                ;

            Mapper.CreateMap<Transfer, Core.Entities.DataIdName>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.TransferId))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));


            Mapper.CreateMap<TransferCategory, Core.Entities.TransferCategory>().ReverseMap();


        }
    }
}
