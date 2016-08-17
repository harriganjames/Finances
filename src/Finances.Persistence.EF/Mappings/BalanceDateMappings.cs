using System;
using AutoMapper;
using Finances.Core.Factories;
using Finances.Interface;

namespace Finances.Persistence.EF.Mappings
{
    public class BalanceDateMappings : IMappingCreator
    {
        readonly IBalanceDateFactory balanceDateFactory;

        public BalanceDateMappings(IBalanceDateFactory balanceDateFactory)
        {
            this.balanceDateFactory = balanceDateFactory;
        }

        public void CreateMappings()
        {

            Mapper.CreateMap<BalanceDateBankAccount, Core.Entities.BalanceDateBankAccount>()
                .ReverseMap()
                .ForMember(t => t.BankAccountId, opt => opt.MapFrom(s => s.BankAccount.BankAccountId))
                .ForMember(t => t.BankAccount, opt => opt.Ignore());

            Mapper.CreateMap<BalanceDate, Core.Entities.BalanceDate>()
                .ConstructUsing((BalanceDate t) => this.balanceDateFactory.Create())
                .ReverseMap()
                .AfterMap((s, d) =>
                 {
                     foreach (var bdba in d.BalanceDateBankAccounts)
                     {
                         bdba.BalanceDateId = s.BalanceDateId;
                     }
                 });



            Mapper.CreateMap<BalanceDate, Core.Entities.DataIdName>()
                .ConvertUsing(bd => new Core.Entities.DataIdName()
                {
                    Id = bd.BalanceDateId,
                    Name = bd.DateOfBalance.ToString()
                });

                //.ForMember(d => d.Id, opt => opt.MapFrom(s => s.BalanceDateId))
                //.ForMember(d => d.Name, opt => opt.ResolveUsing((BalanceDate s) => "abc"));

        }
    }
}
