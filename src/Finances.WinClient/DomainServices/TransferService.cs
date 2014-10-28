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
    public interface ITransferService : IGenericDomainService<Transfer>
    {
        ////bool Add(ITransferEditorViewModel bank);
        ////bool Update(ITransferEditorViewModel bank);
        //bool Delete(int bankId);
        //ITransferItemViewModel Read(int bankId, ITransferItemViewModel bank);
        ////ITransferEditorViewModel Read(int bankId, ITransferEditorViewModel bank);
        List<ITransferItemViewModel> ReadList();
        List<DataIdName> ReadListDataIdName();
        ITransferItemViewModel CreateTransferViewItemModel();
    }

    public class TransferService : GenericDomainService<Transfer>, ITransferService, IDomainService
    {
        readonly ITransferRepository transferRepository;
        readonly IBankAccountService bankAccountService;

        public TransferService(ITransferRepository transferRepository,
                                IBankAccountService bankAccountService
            ) : base(transferRepository)
        {
            this.transferRepository = transferRepository;
            this.bankAccountService = bankAccountService;
        }

        //public bool Add(ITransferEditorViewModel bank)
        //{
        //    Transfer b = new Transfer();
        //    this.bankMapper.Map(bank, b);
        //    int id = this.bankRepository.Add(b);
        //    bank.TransferId = id;
        //    return id>0;
        //}

        //public bool Update(ITransferEditorViewModel bank)
        //{
        //    Transfer b = new Transfer();
        //    this.bankMapper.Map(bank, b);
        //    return this.bankRepository.Update(b);
        //}

        //public bool Delete(int id)
        //{
        //    return this.transferRepository.Delete(new Transfer() { TransferId = id });
        //}


        //public ITransferItemViewModel Read(int id, ITransferItemViewModel transfer)
        //{
        //    Transfer b = this.transferRepository.Read(id);
        //    if (b != null)
        //        transfer.MapIn(b);
        //        //this.transferMapper.Map(b, transfer);
        //    return transfer;
        //}

        //public ITransferEditorViewModel Read(int bankId, ITransferEditorViewModel bank)
        //{
        //    Transfer b = this.transferRepository.Read(bankId);
        //    if (b != null)
        //        this.transferMapper.Map(b, bank);
        //    return bank;
        //}


        public List<ITransferItemViewModel> ReadList()
        {
            List<ITransferItemViewModel> transferVMList = new List<ITransferItemViewModel>();
            List<Transfer> transfers = this.transferRepository.ReadList();
            if (transfers != null)
            {
                foreach (Transfer b in transfers)
                {
                    ITransferItemViewModel vm = this.CreateTransferViewItemModel();
                    //this.transferMapper.Map(b, vm);
                    vm.MapIn(b);
                    transferVMList.Add(vm);
                }
            }
            return transferVMList;
        }


        public List<DataIdName> ReadListDataIdName()
        {
            return this.transferRepository.ReadListDataIdName();
        }


        // possibly move these new's into the parent object (either ctor or prop?)
        public ITransferItemViewModel CreateTransferViewItemModel()
        {
            return new TransferItemViewModel();
            //{
            //    FromBankAccount = this.bankAccountService.CreateBankAccountItemViewModel(),
            //    ToBankAccount = this.bankAccountService.CreateBankAccountItemViewModel()
            //};
        }

    }
}
