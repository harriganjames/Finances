using System;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.WinClient.ViewModels;


namespace Finances.WinClient.Factories
{
    public interface IBankEditorViewModelFactory
    {
        BankEditorViewModel Create(Bank entity);
        void Release(BankEditorViewModel vm);
    }
}
