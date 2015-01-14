using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Finances.Core.Interfaces;
using Finances.WinClient.ViewModels;

namespace Finances.WinClient.DomainServices
{
    public interface IBankAgent
    {
        bool Add(BankEditorViewModel vm);
        bool Delete(BankItemViewModel vm);
        bool Read(int bankId, BankEditorViewModel vm);
        bool Read(int bankId, BankItemViewModel vm);
        List<BankItemViewModel> ReadList();
        bool Update(BankEditorViewModel vm);
    }

    public class BankAgent : IBankAgent
    {
        readonly IBankRepository bankRepository;

        public BankAgent(IBankRepository bankRepository)
        {
            this.bankRepository = bankRepository;
                
        }

        public List<BankItemViewModel> ReadList()
        {
            return Mapper.Map<List<BankItemViewModel>>(bankRepository.ReadList());
        }


        //public void Read<T>(int bankId, T vm)
        //{
        //    var ef = bankRepository.Read(bankId);
        //    Mapper.Map(ef, vm);
        //}

        public bool Read(int bankId, BankItemViewModel vm)
        {
            bool result = false;
            var ef = bankRepository.Read(bankId);
            if (ef != null)
            {
                Mapper.Map(ef, vm);
                result = true;
            }
            return result;
        }

        public bool Read(int bankId, BankEditorViewModel vm)
        {
            bool result = false;
            var ef = bankRepository.Read(bankId);
            if (ef != null)
            {
                Mapper.Map(ef, vm);
                result = true;
            }
            return result;
        }

        public bool Add(BankEditorViewModel vm)
        {
            var ef = Mapper.Map<Core.Entities.Bank>(vm);
            var id = bankRepository.Add(ef);
            Mapper.Map(ef, vm);
            return id > 0;
        }


        public bool Delete(BankItemViewModel vm)
        {
            var ef = Mapper.Map<Core.Entities.Bank>(vm);
            bankRepository.Delete(ef);
            return true;
        }


        public bool Update(BankEditorViewModel vm)
        {
            var ef = Mapper.Map<Core.Entities.Bank>(vm);
            var result = bankRepository.Update(ef);
            if (result)
                Mapper.Map(ef, vm);
            return result;
        }

    }
}
