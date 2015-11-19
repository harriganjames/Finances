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

}
