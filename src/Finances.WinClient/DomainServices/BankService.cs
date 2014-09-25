using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.Interfaces;
using Finances.WinClient.ViewModels;
using Finances.Core.Entities;
using Finances.WinClient.Mappers;

namespace Finances.WinClient.DomainServices
{
    public interface IBankService
    {
        bool Add(IBankEditorViewModel bank);
        bool Update(IBankEditorViewModel bank);
        bool Delete(int bankId);
        //BankItemViewModel Read(int bankId);
        IBankItemViewModel Read(int bankId, IBankItemViewModel bank);
        IBankEditorViewModel Read(int bankId, IBankEditorViewModel bank);
        List<IBankItemViewModel> ReadList();
        List<DataIdName> ReadListDataIdName();
        IBankItemViewModel CreateBankViewModel();
    }

    public class BankService : IBankService, IDomainService
    {
        readonly IBankRepository bankRepository;
        readonly IBankMapper bankMapper;
        //readonly IBankViewModelFactory bankViewModelFactory;

        public BankService(IBankRepository bankRepository,
                        IBankMapper bankMapper)
        {
            this.bankRepository = bankRepository;
            this.bankMapper = bankMapper;
            //this.bankViewModelFactory = bankViewModelFactory;
        }

        public bool Add(IBankEditorViewModel bank)
        {
            Bank b = new Bank();
            this.bankMapper.Map(bank, b);
            int id = this.bankRepository.Add(b);
            bank.BankId = id;
            return id>0;
        }

        public bool Update(IBankEditorViewModel bank)
        {
            Bank b = new Bank();
            this.bankMapper.Map(bank, b);
            return this.bankRepository.Update(b);
        }

        public bool Delete(int bankId)
        {
            //Bank b = new Bank();
            //this.bankMapper.Map(bank, b);
            return this.bankRepository.Delete(bankId);
        }



        //public BankItemViewModel Read(int bankId)
        //{
        //    BankItemViewModel vm = this.CreateBankViewModel();
        //    return this.Read(bankId, vm);
        //}


        public IBankItemViewModel Read(int bankId, IBankItemViewModel bank)
        {
            Bank b = this.bankRepository.Read(bankId);
            if(b!=null)
                this.bankMapper.Map(b, bank);
            return bank;
        }

        public IBankEditorViewModel Read(int bankId, IBankEditorViewModel bank)
        {
            Bank b = this.bankRepository.Read(bankId);
            if (b != null)
                this.bankMapper.Map(b, bank);
            return bank;
        }


        public List<IBankItemViewModel> ReadList()
        {
            List<IBankItemViewModel> bankVMList = new List<IBankItemViewModel>();
            List<Bank> banks = this.bankRepository.ReadList();
            if (banks != null)
            {
                foreach (Bank b in banks)
                {
                    IBankItemViewModel vm = this.CreateBankViewModel();
                    this.bankMapper.Map(b, vm);
                    bankVMList.Add(vm);
                }
            }
            return bankVMList;
        }


        public List<DataIdName> ReadListDataIdName()
        {
            return this.bankRepository.ReadListDataIdName();
        }

        //public BankViewModel CreateBankViewModel()
        //{
        //    return this.bankViewModelFactory.CreateBankViewModel();
        //}

        public IBankItemViewModel CreateBankViewModel()
        {
            return new BankItemViewModel();
        }

    }
}
