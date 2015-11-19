using System;
//using AutoMapper;
using Finances.Core.Entities;
//using Finances.Core.Interfaces;
using Finances.WinClient.ViewModels;


namespace Finances.WinClient.Factories
{
    public interface IBankAccountEditorViewModelFactory
    {
        BankAccountEditorViewModel Create(BankAccount entity);
        void Release(BankAccountEditorViewModel vm);
    }

}
