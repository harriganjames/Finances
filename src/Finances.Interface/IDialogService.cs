using System;
using System.Windows.Input;

namespace Finances.Interface
{
    public interface IDialogService
    {
        //bool ShowDialog(object vm);
        bool ShowDialog(IDialog vm);
        bool ShowDialogView(IDialog vm);
        void ShowDialogNonModal(IDialog vm);
        ICommand DialogAcceptCommand { get; }
        MessageBoxResultEnum ShowMessageBox(string title, string message, MessageBoxButtonEnum buttons);
        string[] ShowOpenFileDialog(string filter = "", bool multi = false);
    }

}
