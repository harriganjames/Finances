using System;
using AutoMapper;
using Finances.Core.Interfaces;
using Finances.Core.Wpf;
using Finances.WinClient.ViewModels;


namespace Finances.WinClient.Factories
{
    public interface IBankEditorViewModelFactory
    {
        BankEditorViewModel Create();
        void Release(BankEditorViewModel vm);
    }

    //public class IBankEditorViewModelFactory : IBankEditorViewModelFactory
    //{
    //    readonly IBankRepository bankRepository;
    //    readonly IDialogService dialogService;

    //    public IBankEditorViewModelFactory(
    //            IBankRepository bankRepository,
    //            IDialogService dialogService
    //            )
    //    {
    //        this.bankRepository = bankRepository;
    //        this.dialogService = dialogService;
    //    }

    //    public BankEditorViewModel Create()
    //    {
    //        return new BankEditorViewModel(bankRepository, dialogService);
    //    }

    //}
}
