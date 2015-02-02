using AutoMapper;
using Finances.Core.Interfaces;
using Finances.WinClient.ViewModels;

namespace Finances.WinClient.DomainServices
{

    public class MappingCreator : IMappingCreator
    {
        public void CreateMappings()
        {
            //Banks
            //Mapper.CreateMap<Core.Entities.Bank, BankItemViewModel>()
            //        .ForMember(b => b.LogoRaw, opt => opt.MapFrom(s => s.Logo))
            //        .ReverseMap()
            //        .ForMember(s => s.Logo, opt => opt.MapFrom(b => b.LogoRaw));

            // temp until remove interfaces for non-DI classes
            {
                Mapper.CreateMap<Core.Entities.Bank, IBankItemViewModel>()
                        .ForMember(b => b.LogoRaw, opt => opt.MapFrom(s => s.Logo));

                Mapper.CreateMap<IBankItemViewModel, Core.Entities.Bank>()
                        .ForMember(s => s.Logo, opt => opt.MapFrom(b => b.LogoRaw));
            }


            Mapper.CreateMap<Core.Entities.Bank, BankItemViewModel>()
                    .ForMember(b => b.LogoRaw, opt => opt.MapFrom(s => s.Logo));

            Mapper.CreateMap<BankItemViewModel, Core.Entities.Bank>()
                    .ForMember(s => s.Logo, opt => opt.MapFrom(b => b.LogoRaw));


            //Mapper.CreateMap<BankItemViewModel, Core.Entities.Bank>()
            //        .ForMember(b => b.Logo, opt => opt.MapFrom(s => s.LogoRaw));



            //    .ConstructUsing((BankItemViewModel bvm) =>
            //{
            //    return new Core.Entities.Bank() { BankId = bvm.BankId };
            //}); // only need the BankId

            Mapper.CreateMap<Core.Entities.Bank, BankEditorViewModel>()
                    .ForMember(b => b.LogoRaw, opt => opt.MapFrom(s => s.Logo))
                    .ReverseMap()
                    .ForMember(s => s.Logo, opt => opt.MapFrom(b => b.LogoRaw));

            //Mapper.CreateMap<BankEditorViewModel, Core.Entities.Bank>();


            // Bank Account

            Mapper.CreateMap<Core.Entities.BankAccount, BankAccountItemViewModel>()
                    .ForMember(a => a.AccountName, opt => opt.MapFrom(s => s.Name))
                    .ReverseMap()
                    .ForMember(a => a.Name, opt => opt.MapFrom(s => s.AccountName));

            Mapper.CreateMap<Core.Entities.BankAccount, BankAccountEditorViewModel>()
                    .ForMember(a => a.AccountName, opt => opt.MapFrom(s => s.Name))
                    .ReverseMap()
                    .ForMember(a => a.Name, opt => opt.MapFrom(s => s.AccountName));


            // temp until remove interfaces for non-DI classes
            {
                Mapper.CreateMap<Core.Entities.BankAccount, IBankAccountItemViewModel>()
                        .ForMember(a => a.AccountName, opt => opt.MapFrom(s => s.Name));

                Mapper.CreateMap<IBankAccountItemViewModel, Core.Entities.BankAccount>()
                        .ForMember(a => a.Name, opt => opt.MapFrom(s => s.AccountName));
            }

            // Transfers

            Mapper.CreateMap<Core.Entities.Transfer, TransferItemViewModel>() // below is temo until Interfaces removed??
                    .ForMember(a => a.FromBankAccount, opt => opt.MapFrom(s => s.FromBankAccount == null ? null : Mapper.Map<BankAccountItemViewModel>(s.FromBankAccount)))
                    .ForMember(a => a.ToBankAccount, opt => opt.MapFrom(s => s.ToBankAccount == null ? null : Mapper.Map<BankAccountItemViewModel>(s.ToBankAccount)))
                    .ReverseMap();

            Mapper.CreateMap<Core.Entities.Transfer, TransferEditorViewModel>() // below is temp until Interfaces removed??
                    .ForMember(a => a.FromBankAccount, 
                            opt => opt.MapFrom(
                                s => s.FromBankAccount == null ? BankAccountItemViewModel.Elsewhere : Mapper.Map<BankAccountItemViewModel>(s.FromBankAccount)
                                ))
                    .ForMember(a => a.ToBankAccount, 
                            opt => opt.MapFrom(
                                s => s.ToBankAccount == null ? BankAccountItemViewModel.Elsewhere : Mapper.Map<BankAccountItemViewModel>(s.ToBankAccount)
                                ))
                    .ForMember(a => a.Amount, 
                            opt => opt.MapFrom(s => new InputDecimal() { Value=s.Amount }))
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


        }
    }

}
