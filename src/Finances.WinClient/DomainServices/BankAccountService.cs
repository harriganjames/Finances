using System.Linq;
using Finances.Core.Interfaces;
using Finances.WinClient.ViewModels;
using Finances.Core.Entities;
using System.Collections.Generic;

namespace Finances.WinClient.DomainServices
{
    public interface IBankAccountService : IGenericDomainService<BankAccount>
    {
        //bool Add(IBankAccountEditorViewModel bankAccount);
        //bool Update(IBankAccountEditorViewModel bankAccount);
        //bool Delete(int bankAccountId);
        //IBankAccountItemViewModel Read(int bankAccountId, IBankAccountItemViewModel bankAccount);
        //IBankAccountEditorViewModel Read(int bankAccountId, IBankAccountEditorViewModel bankAccount);
        List<IBankAccountItemViewModel> ReadList();
        List<IBankAccountItemViewModel> ReadListByBankId(int bankId);
        List<DataIdName> ReadListDataIdName();
        IBankAccountItemViewModel CreateBankAccountItemViewModel();
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

        //public bool Add(IBankAccountEditorViewModel bankAccount)
        //{
        //    BankAccount b = new BankAccount();
        //    this.bankAccountMapper.Map(bankAccount, b);
        //    int id = this.bankAccountRepository.Add(b);
        //    bankAccount.BankAccountId = id;
        //    return id > 0;
        //}

        //public bool Update(IBankAccountEditorViewModel bankAccount)
        //{
        //    BankAccount b = new BankAccount();
        //    this.bankAccountMapper.Map(bankAccount, b);
        //    return this.bankAccountRepository.Update(b);
        //}

        //public bool Delete(int bankAccountId)
        //{
        //    return this.bankAccountRepository.Delete(new BankAccount() { BankAccountId=bankAccountId });
        //}


        //public IBankAccountItemViewModel Read(int bankAccountId, IBankAccountItemViewModel bankAccount)
        //{
        //    BankAccount b = this.bankAccountRepository.Read(bankAccountId);
        //    if (b != null)
        //        this.bankAccountMapper.Map(b, bankAccount);
        //    return bankAccount;
        //}

        //public IBankAccountEditorViewModel Read(int bankAccountId, IBankAccountEditorViewModel bankAccount)
        //{
        //    BankAccount b = this.bankAccountRepository.Read(bankAccountId);
        //    if (b != null)
        //        this.bankAccountMapper.Map(b, bankAccount);
        //    return bankAccount;
        //}


        public List<IBankAccountItemViewModel> ReadList()
        {
            List<IBankAccountItemViewModel> bankAccountVMList = new List<IBankAccountItemViewModel>();
            List<BankAccount> bankAccounts = this.bankAccountRepository.ReadList();
            if (bankAccounts != null)
            {
                foreach (BankAccount b in bankAccounts)
                {
                    IBankAccountItemViewModel vm = this.CreateBankAccountItemViewModel();
                    vm.MapIn(b);
                    //this.bankAccountMapper.Map(b, vm);
                    bankAccountVMList.Add(vm);
                }
            }
            return bankAccountVMList;
        }

        public List<IBankAccountItemViewModel> ReadListByBankId(int bankId)
        {
            List<IBankAccountItemViewModel> bankAccountVMList = new List<IBankAccountItemViewModel>();
            var bankAccounts = this.bankAccountRepository.ReadList().Where(a=>a.Bank.BankId==bankId);
            if (bankAccounts != null)
            {
                foreach (BankAccount b in bankAccounts)
                {
                    IBankAccountItemViewModel vm = this.CreateBankAccountItemViewModel();
                    vm.MapIn(b);
                    bankAccountVMList.Add(vm);
                }
            }
            return bankAccountVMList;
        }

        public List<DataIdName> ReadListDataIdName()
        {
            return this.bankAccountRepository.ReadListDataIdName();
        }


        public IBankAccountItemViewModel CreateBankAccountItemViewModel()
        {
            return new BankAccountItemViewModel();
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
