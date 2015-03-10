using System;
using AutoMapper;
using Finances.Core.Interfaces;
using Finances.WinClient.ViewModels;


namespace Finances.WinClient.Factories
{
    public interface ICashflowEditorViewModelFactory
    {
        CashflowEditorViewModel Create();
        void Release(CashflowEditorViewModel vm);
    }

}
