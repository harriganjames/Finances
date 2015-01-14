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
        List<ITransferItemViewModel> ReadList();
        List<DataIdName> ReadListDataIdName();
    }

    public class TransferService : GenericDomainService<Transfer>, ITransferService, IDomainService
    {
        readonly ITransferRepository transferRepository;

        public TransferService(ITransferRepository transferRepository
                    ) : base(transferRepository)
        {
            this.transferRepository = transferRepository;
        }



        public List<ITransferItemViewModel> ReadList()
        {
            return base.ReadList<TransferItemViewModel,ITransferItemViewModel>();
        }



        public List<DataIdName> ReadListDataIdName()
        {
            return this.transferRepository.ReadListDataIdName();
        }


    }
}
