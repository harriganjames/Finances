using Finances.Core.Interfaces;
using Finances.WinClient.ViewModels;
using Finances.Core.Entities;
using Finances.WinClient.Mappers;
using System.Collections.Generic;

namespace Finances.WinClient.DomainServices
{
    public interface IBankAccountService
    {
        bool Add(IBankAccountEditorViewModel bankAccount);
        bool Update(IBankAccountEditorViewModel bankAccount);
        bool Delete(int bankAccountId);
        IBankAccountItemViewModel Read(int bankAccountId, IBankAccountItemViewModel bankAccount);
        IBankAccountEditorViewModel Read(int bankAccountId, IBankAccountEditorViewModel bankAccount);
        List<IBankAccountItemViewModel> ReadList();
        List<DataIdName> ReadListDataIdName();
        IBankAccountItemViewModel CreateBankAccountViewModel();
    }

    public class BankAccountService : IBankAccountService, IDomainService
    {
        readonly IBankAccountRepository bankAccountRepository;
        readonly IBankAccountMapper bankAccountMapper;

        public BankAccountService(IBankAccountRepository bankAccountRepository,
                        IBankAccountMapper bankAccountMapper)
        {
            this.bankAccountRepository = bankAccountRepository;
            this.bankAccountMapper = bankAccountMapper;
        }

        public bool Add(IBankAccountEditorViewModel bankAccount)
        {
            BankAccount b = new BankAccount();
            this.bankAccountMapper.Map(bankAccount, b);
            int id = this.bankAccountRepository.Add(b);
            bankAccount.BankAccountId = id;
            return id > 0;
        }

        public bool Update(IBankAccountEditorViewModel bankAccount)
        {
            BankAccount b = new BankAccount();
            this.bankAccountMapper.Map(bankAccount, b);
            return this.bankAccountRepository.Update(b);
        }

        public bool Delete(int bankAccountId)
        {
            return this.bankAccountRepository.Delete(bankAccountId);
        }


        public IBankAccountItemViewModel Read(int bankAccountId, IBankAccountItemViewModel bankAccount)
        {
            BankAccount b = this.bankAccountRepository.Read(bankAccountId);
            if (b != null)
                this.bankAccountMapper.Map(b, bankAccount);
            return bankAccount;
        }

        public IBankAccountEditorViewModel Read(int bankAccountId, IBankAccountEditorViewModel bankAccount)
        {
            BankAccount b = this.bankAccountRepository.Read(bankAccountId);
            if (b != null)
                this.bankAccountMapper.Map(b, bankAccount);
            return bankAccount;
        }


        public List<IBankAccountItemViewModel> ReadList()
        {
            List<IBankAccountItemViewModel> bankAccountVMList = new List<IBankAccountItemViewModel>();
            List<BankAccount> bankAccounts = this.bankAccountRepository.ReadList();
            if (bankAccounts != null)
            {
                foreach (BankAccount b in bankAccounts)
                {
                    IBankAccountItemViewModel vm = this.CreateBankAccountViewModel();
                    this.bankAccountMapper.Map(b, vm);
                    bankAccountVMList.Add(vm);
                }
            }
            return bankAccountVMList;
        }


        public List<DataIdName> ReadListDataIdName()
        {
            return this.bankAccountRepository.ReadListDataIdName();
        }


        public IBankAccountItemViewModel CreateBankAccountViewModel()
        {
            return new BankAccountItemViewModel() { Bank = new BankItemViewModel() };
        }

    }


}
