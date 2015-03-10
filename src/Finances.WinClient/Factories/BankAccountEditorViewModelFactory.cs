using System;
using AutoMapper;
using Finances.Core.Interfaces;
using Finances.WinClient.ViewModels;


namespace Finances.WinClient.Factories
{
    public interface IBankAccountEditorViewModelFactory
    {
        BankAccountEditorViewModel Create();
        void Release(BankAccountEditorViewModel vm);
    }

    //public class IBankAccountEditorViewModelFactory : IBankAccountEditorViewModelFactory
    //{
    //    readonly IBankRepository bankRepository;
    //    readonly IMappingEngine mapper;
    //    readonly IBankAccountRepository bankAccountRepository;

    //    public IBankAccountEditorViewModelFactory(
    //            IBankAccountRepository bankAccountRepository,
    //            IBankRepository bankRepository,
    //            IMappingEngine mapper
    //            )
    //    {
    //        this.bankAccountRepository = bankAccountRepository;
    //        this.bankRepository = bankRepository;
    //        this.mapper = mapper;
    //    }

    //    public BankAccountEditorViewModel Create()
    //    {
    //        return new BankAccountEditorViewModel(bankAccountRepository, bankRepository, mapper);
    //    }


    //}
}
