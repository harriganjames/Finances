using System;
using Finances.WinClient.DomainServices;
using Finances.WinClient.ViewModels;


namespace Finances.WinClient.Factories
{
    public interface ITransferEditorViewModelFactory
    {
        ITransferEditorViewModel Create();
    }

    public class TransferEditorViewModelFactory : ITransferEditorViewModelFactory
    {
        readonly ITransferService transferService;
        readonly IBankAccountService bankAccountService;

        public TransferEditorViewModelFactory(ITransferService transferService,
                    IBankAccountService bankAccountService)
        {
            this.transferService = transferService;
            this.bankAccountService = bankAccountService;

        }

        public ITransferEditorViewModel Create()
        {
            return new TransferEditorViewModel(transferService, bankAccountService);
        }


    }
}
