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
        List<IBankItemViewModel> ReadList();
        List<DataIdName> ReadListDataIdName();
    }

    public class BankService : GenericDomainService<Bank> ,IBankService, IDomainService
    {
        readonly IBankRepository bankRepository;

        public BankService(IBankRepository bankRepository) : base(bankRepository)
        {
            this.bankRepository = bankRepository;
        }



        public List<IBankItemViewModel> ReadList()
        {
            return base.ReadList<BankItemViewModel, IBankItemViewModel>();
        }




        public List<DataIdName> ReadListDataIdName()
        {
            return this.bankRepository.ReadListDataIdName();
        }


    }
}
