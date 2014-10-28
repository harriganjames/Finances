using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.Interfaces;
using Finances.WinClient.ViewModels;
using Finances.Core.Entities;

namespace Finances.WinClient.DomainServices
{
    public interface IBankService : IGenericDomainService<Bank>
    {
        //bool Add(IBankEditorViewModel bank);
        //bool Update(IBankEditorViewModel bank);
        //bool Delete(int bankId);
        //BankItemViewModel Read(int bankId);
        //IBankItemViewModel Read(int bankId, IBankItemViewModel bank);
        //IBankEditorViewModel Read(int bankId, IBankEditorViewModel bank);
        List<IBankItemViewModel> ReadList();
        List<DataIdName> ReadListDataIdName();
        IBankItemViewModel CreateBankItemViewModel();
    }

    public class BankService : GenericDomainService<Bank> ,IBankService, IDomainService
    {
        readonly IBankRepository bankRepository;

        public BankService(IBankRepository bankRepository) : base(bankRepository)
        {
            this.bankRepository = bankRepository;
        }

        //public bool Add(IBankEditorViewModel bank)
        //{
        //    Bank b = new Bank();
        //    //this.bankMapper.Map(bank, b);
        //    bank.MapOut(b);
        //    int id = this.bankRepository.Add(b);
        //    bank.BankId = id;
        //    return id>0;
        //}


        //public bool Update(IBankEditorViewModel bank)
        //{
        //    Bank b = new Bank();
        //    //this.bankMapper.Map(bank, b);
        //    bank.MapOut(b);
        //    return this.bankRepository.Update(b);
        //}

        //public bool Delete(int bankId)
        //{
        //    //Bank b = new Bank();
        //    //this.bankMapper.Map(bank, b);
        //    return this.bankRepository.Delete(new Bank() { BankId = bankId });
        //}


        //public BankItemViewModel Read(int bankId)
        //{
        //    BankItemViewModel vm = this.CreateBankViewModel();
        //    return this.Read(bankId, vm);
        //}


        //public IBankItemViewModel Read(int bankId, IBankItemViewModel bank)
        //{
        //    Bank b = this.bankRepository.Read(bankId);
        //    if (b != null)
        //        bank.MapIn(b);
        //        //this.bankMapper.Map(b, bank);
        //    return bank;
        //}

        //public void Read<T>(int bankId, T bank) where T : IBankMapIn
        //{
        //    Bank b = this.bankRepository.Read(bankId);
        //    if (b != null)
        //        bank.MapIn(b);
        //}


        //public IBankEditorViewModel Read(int bankId, IBankEditorViewModel bank)
        //{
        //    Bank b = this.bankRepository.Read(bankId);
        //    if (b != null)
        //        bank.MapIn(b);
        //        //this.bankMapper.Map(b, bank);
        //    return bank;
        //}


        public List<IBankItemViewModel> ReadList()
        {
            List<IBankItemViewModel> bankVMList = new List<IBankItemViewModel>();
            List<Bank> banks = this.bankRepository.ReadList();
            if (banks != null)
            {
                foreach (Bank b in banks)
                {
                    IBankItemViewModel vm = this.CreateBankItemViewModel();
                    //this.bankMapper.Map(b, vm);
                    vm.MapIn(b);
                    bankVMList.Add(vm);
                }
            }
            return bankVMList;
        }



        //public List<T> ReadList<T>(Func<T> create) where T : IBankMapIn
        //{
        //    List<T> bankVMList = new List<T>();
        //    List<Bank> banks = this.bankRepository.ReadList();
        //    if (banks != null)
        //    {
        //        foreach (Bank b in banks)
        //        {
        //            T vm = create();
        //            vm.MapIn(b);
        //            bankVMList.Add(vm);
        //        }
        //    }
        //    return bankVMList;
        //}


        public List<DataIdName> ReadListDataIdName()
        {
            return this.bankRepository.ReadListDataIdName();
        }


        public IBankItemViewModel CreateBankItemViewModel()
        {
            return new BankItemViewModel();
        }

        //public T CreateBankItemViewModel<T>() where T: IBankMapIn, new()
        //{
        //    return new T();
        //}


    }
}
