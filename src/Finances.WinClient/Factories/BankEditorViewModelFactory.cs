using System;
using AutoMapper;
using Finances.Core.Interfaces;
using Finances.Core.Wpf;
using Finances.WinClient.ViewModels;


namespace Finances.WinClient.Factories
{
    public interface IBankEditorViewModelFactory
    {
        IBankEditorViewModel Create();
        void Release(IBankEditorViewModel vm);
    }

    //public class BankEditorViewModelFactory : IBankEditorViewModelFactory
    //{
    //    readonly IBankRepository bankRepository;
    //    readonly IDialogService dialogService;

    //    public BankEditorViewModelFactory(
    //            IBankRepository bankRepository,
    //            IDialogService dialogService
    //            )
    //    {
    //        this.bankRepository = bankRepository;
    //        this.dialogService = dialogService;
    //    }

    //    public IBankEditorViewModel Create()
    //    {
    //        return new BankEditorViewModel(bankRepository, dialogService);
    //    }

    //}
}
