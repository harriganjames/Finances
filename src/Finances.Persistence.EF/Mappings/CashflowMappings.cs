using System;
using AutoMapper;
using Finances.Core.Factories;
using Finances.Interface;

namespace Finances.Persistence.EF.Mappings
{
    public class CashflowMappings : IMappingCreator
    {
        readonly ICashflowFactory cashflowFactory;

        public CashflowMappings(ICashflowFactory cashflowFactory)
        {
            this.cashflowFactory = cashflowFactory;
        }

        public void CreateMappings()
        {
            Mapper.CreateMap<CashflowBankAccount, Core.Entities.CashflowBankAccount>()
                .ReverseMap()
                .ForMember(t => t.BankAccountId, opt => opt.MapFrom(s => s.BankAccount.BankAccountId))
                .ForMember(t => t.BankAccount, opt => opt.Ignore());

            Mapper.CreateMap<Cashflow, Core.Entities.Cashflow>()
                .ConstructUsing((Cashflow t) => this.cashflowFactory.Create())
                .ReverseMap()
                .AfterMap((s, d) =>
                {
                    foreach (var cba in d.CashflowBankAccounts)
                    {
                        cba.CashflowId = s.CashflowId;
                    }
                });

            Mapper.CreateMap<Cashflow, Core.Entities.DataIdName>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.CashflowId))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));

        }
    }
}
