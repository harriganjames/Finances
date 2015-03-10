using System.Linq;
using AutoMapper;
using Finances.Core.Interfaces;
using Finances.WinClient.ViewModels;

namespace Finances.WinClient.DomainServices
{

    public class MappingCreator : IMappingCreator
    {
        //private class InputDecimalResolver : IValueResolver
        //{
        //    public ResolutionResult Resolve(ResolutionResult source)
        //    {
        //        var ind = (InputDecimal)source.Context.DestinationValue;

        //        ind.Value = (decimal)source.Value;

        //        return source.New(ind, typeof(InputDecimal));
        //    }
        //}

        
        public void CreateMappings()
        {
            //Banks

            Mapper.CreateMap<Core.Entities.Bank, BankItemViewModel>()
                    .ForMember(b => b.LogoRaw, opt => opt.MapFrom(s => s.Logo))
                    .ReverseMap()
                    .ForMember(s => s.Logo, opt => opt.MapFrom(b => b.LogoRaw));

            //Mapper.CreateMap<BankItemViewModel, Core.Entities.Bank>()
            //        .ForMember(s => s.Logo, opt => opt.MapFrom(b => b.LogoRaw));



            Mapper.CreateMap<Core.Entities.Bank, BankEditorViewModel>()
                    .ForMember(b => b.LogoRaw, opt => opt.MapFrom(s => s.Logo))
                    .ReverseMap()
                    .ForMember(s => s.Logo, opt => opt.MapFrom(b => b.LogoRaw));



            // Bank Account

            Mapper.CreateMap<Core.Entities.BankAccount, BankAccountItemViewModel>()
                    .ForMember(a => a.AccountName, opt => opt.MapFrom(s => s.Name))
                    .ReverseMap()
                    .ForMember(a => a.Name, opt => opt.MapFrom(s => s.AccountName));

            Mapper.CreateMap<Core.Entities.BankAccount, BankAccountEditorViewModel>()
                    .ForMember(a => a.AccountName, opt => opt.MapFrom(s => s.Name))
                    .ReverseMap()
                    .ForMember(a => a.Name, opt => opt.MapFrom(s => s.AccountName));



            // Transfers

            Mapper.CreateMap<Core.Entities.Transfer, TransferItemViewModel>() 
                    .ReverseMap();

            Mapper.CreateMap<Core.Entities.Transfer, TransferEditorViewModel>() 
                    .ForMember(a => a.FromBankAccount, 
                            opt => opt.MapFrom(
                                s => s.FromBankAccount == null ? BankAccountItemViewModel.Elsewhere : Mapper.Map<BankAccountItemViewModel>(s.FromBankAccount)
                                ))
                    .ForMember(a => a.ToBankAccount, 
                            opt => opt.MapFrom(
                                s => s.ToBankAccount == null ? BankAccountItemViewModel.Elsewhere : Mapper.Map<BankAccountItemViewModel>(s.ToBankAccount)
                                ))
                    //.ForMember(a => a.Amount, 
                    //        opt => opt.MapFrom(s => new InputDecimal() { Value=s.Amount }))
                    .AfterMap((e, vm) => vm.Amount.Value = e.Amount)
                    .ReverseMap()
                    .ForMember(a => a.Amount, opt => 
                            opt.MapFrom(s => s.Amount!=null ? s.Amount.Value : 0M))
                    .ForMember(a => a.FromBankAccount, 
                            opt => opt.MapFrom(
                                s => s.FromBankAccount.BankAccountId==BankAccountItemViewModel.Elsewhere.BankAccountId ? null : Mapper.Map<Core.Entities.BankAccount>(s.FromBankAccount)
                                ))
                    .ForMember(a => a.ToBankAccount,
                            opt => opt.MapFrom(
                                s => s.ToBankAccount.BankAccountId == BankAccountItemViewModel.Elsewhere.BankAccountId ? null : Mapper.Map<Core.Entities.BankAccount>(s.ToBankAccount)
                                ))
                    ;


            // Cashflows
            Mapper.CreateMap<Core.Entities.Cashflow, Core.Entities.Cashflow>();

            Mapper.CreateMap<Core.Entities.CashflowBankAccount, CashflowBankAccountItemViewModel>()
                    .ReverseMap();

            Mapper.CreateMap<Core.Entities.Cashflow, CashflowItemViewModel>()
                    .ReverseMap();


            //Mapper.CreateMap<decimal, InputDecimal>()
            //    .ConvertUsing(a => new InputDecimal() { Value = a });

            //Mapper.CreateMap<InputDecimal, decimal>()
            //    .ConvertUsing(a => a.Value);


            Mapper.CreateMap<Core.Entities.Cashflow, CashflowEditorViewModel>()
                    //.ForMember(a => a.OpeningBalance, opt => opt.ResolveUsing<InputDecimalResolver>())
                    .AfterMap((e,vm) => vm.OpeningBalance.Value = e.OpeningBalance)
                    .ReverseMap()
                    .ForMember(a => a.CashflowBankAccounts, opt =>
                            opt.MapFrom(s => s.BankAccounts.Where(ba=>ba.IsSelected)))
                    .ForMember(a => a.OpeningBalance, opt =>
                            opt.MapFrom(s => s.OpeningBalance != null ? s.OpeningBalance.Value : 0M))
                    ;



        }
    }

}
