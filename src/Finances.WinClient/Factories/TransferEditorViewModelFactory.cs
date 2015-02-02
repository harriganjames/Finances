using System;
using AutoMapper;
using Finances.Core.Interfaces;
using Finances.WinClient.ViewModels;


namespace Finances.WinClient.Factories
{
    public interface ITransferEditorViewModelFactory
    {
        ITransferEditorViewModel Create();
        void Release(ITransferEditorViewModel vm);
    }

    //public class TransferEditorViewModelFactory : ITransferEditorViewModelFactory
    //{

    //    readonly ITransferRepository transferRepository;
    //    readonly IMappingEngine mapper;
    //    readonly IBankAccountRepository bankAccountRepository;

    //    public TransferEditorViewModelFactory(
    //            ITransferRepository transferRepository,
    //            IMappingEngine mapper,
    //            IBankAccountRepository bankAccountRepository
    //            )
    //    {
    //        this.transferRepository = transferRepository;
    //        this.mapper = mapper;
    //        this.bankAccountRepository = bankAccountRepository;
    //    }

    //    public ITransferEditorViewModel Create()
    //    {
    //        return new TransferEditorViewModel(transferRepository, mapper, bankAccountRepository);
    //    }
    //}
}
