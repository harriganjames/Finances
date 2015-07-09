using System;
//using AutoMapper;
using Finances.Core.Entities;
//using Finances.Core.Interfaces;
using Finances.WinClient.ViewModels;


namespace Finances.WinClient.Factories
{
    public interface ITransferEditorViewModelFactory
    {
        TransferEditorViewModel Create(Transfer entity);
        void Release(TransferEditorViewModel vm);
    }

    //public class ITransferEditorViewModelFactory : ITransferEditorViewModelFactory
    //{

    //    readonly ITransferRepository transferRepository;
    //    readonly IMappingEngine mapper;
    //    readonly IBankAccountRepository bankAccountRepository;

    //    public ITransferEditorViewModelFactory(
    //            ITransferRepository transferRepository,
    //            IMappingEngine mapper,
    //            IBankAccountRepository bankAccountRepository
    //            )
    //    {
    //        this.transferRepository = transferRepository;
    //        this.mapper = mapper;
    //        this.bankAccountRepository = bankAccountRepository;
    //    }

    //    public TransferEditorViewModel Create()
    //    {
    //        return new TransferEditorViewModel(transferRepository, mapper, bankAccountRepository);
    //    }
    //}
}
