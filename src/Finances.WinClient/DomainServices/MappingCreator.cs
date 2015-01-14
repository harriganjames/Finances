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

            // temp
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


        }
    }

}
