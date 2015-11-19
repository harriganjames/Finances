using System;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.WinClient.ViewModels;

namespace Finances.WinClient.Factories
{
    public interface ICashflowEditorViewModelFactory
    {
        CashflowEditorViewModel Create(Cashflow entity);
        void Release(CashflowEditorViewModel vm);
    }
}
