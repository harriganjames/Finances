using System.Linq;
using Finances.Core.Interfaces;
using Finances.WinClient.ViewModels;
using Finances.Core.Entities;
using System.Collections.Generic;

namespace Finances.WinClient.DomainServices
{
    public interface IBankAccountService : IGenericDomainService<BankAccount>
    {
        List<IBankAccountItemViewModel> ReadList();
        List<IBankAccountItemViewModel> ReadListByBankId(int bankId);
        List<DataIdName> ReadListDataIdName();
        List<IBankItemViewModel> ReadBankList();
    }

    public class BankAccountService : GenericDomainService<BankAccount>, IBankAccountService, IDomainService
    {
        readonly IBankAccountRepository bankAccountRepository;
        readonly IBankRepository bankRepository;

        public BankAccountService(  IBankAccountRepository bankAccountRepository,
                                    IBankRepository bankRepository
                            ) : base(bankAccountRepository)
        {
            this.bankAccountRepository = bankAccountRepository;
            this.bankRepository = bankRepository;
        }



        public List<IBankAccountItemViewModel> ReadList()
        {
            return base.ReadList<BankAccountItemViewModel, IBankAccountItemViewModel>();
        }


        public List<IBankAccountItemViewModel> ReadListByBankId(int bankId)
        {
            return base.ReadListFunc<BankAccountItemViewModel, IBankAccountItemViewModel>( () => this.bankAccountRepository.ReadListByBankId(bankId) );
        }


        public List<DataIdName> ReadListDataIdName()
        {
            return this.bankAccountRepository.ReadListDataIdName();
        }



        #region Bank

        public List<IBankItemViewModel> ReadBankList()
        {
            List<IBankItemViewModel> bankVMList = new List<IBankItemViewModel>();
            List<Bank> banks = this.bankRepository.ReadList();
            if (banks != null)
            {
                foreach (Bank b in banks)
                {
                    IBankItemViewModel vm = new BankItemViewModel();
                    //this.bankMapper.Map(b, vm);
                    vm.MapIn(b);
                    bankVMList.Add(vm);
                }
            }
            return bankVMList;
        }

        #endregion



    }


}
