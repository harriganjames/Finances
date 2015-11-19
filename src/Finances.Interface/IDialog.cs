using System;
using System.Windows.Input;

namespace Finances.Interface
{
    public interface IDialog
    {
        ICommand DialogAcceptCommand { get; }
        void DialogOkClicked();
    }
}
