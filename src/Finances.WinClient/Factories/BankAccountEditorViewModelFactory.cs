using System;
using AutoMapper;
using Finances.Core.Interfaces;
using Finances.WinClient.ViewModels;


namespace Finances.WinClient.Factories
{
    public interface IBankAccountEditorViewModelFactory
    {
        IBankAccountEditorViewModel Create();
        void Release(IBankAccountEditorViewModel vm);
    }

    //public class BankAccountEditorViewModelFactory : IBankAccountEditorViewModelFactory
    //{
    //    readonly IBankRepository bankRepository;
    //    readonly IMappingEngine mapper;
    //    readonly IBankAccountRepository bankAccountRepository;

    //    public BankAccountEditorViewModelFactory(
    //            IBankAccountRepository bankAccountRepository,
    //            IBankRepository bankRepository,
    //            IMappingEngine mapper
    //            )
    //    {
    //        this.bankAccountRepository = bankAccountRepository;
    //        this.bankRepository = bankRepository;
    //        this.mapper = mapper;
    //    }

    //    public IBankAccountEditorViewModel Create()
    //    {
    //        return new BankAccountEditorViewModel(bankAccountRepository, bankRepository, mapper);
    //    }


    //}
}
