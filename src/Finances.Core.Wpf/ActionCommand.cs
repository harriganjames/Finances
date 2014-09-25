using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Finances.Core.Wpf
{
    public class ActionCommand : ICommand
    {
        Action _execute;
        Action<object> _executeParam;
        Func<bool> _can;
        Func<object,bool> _canParam;

        #region Constructors
        public ActionCommand(Action execute)
        {
            _execute = execute;
        }
        public ActionCommand(Action<object> execute)
        {
            _executeParam = execute;
        }
        public ActionCommand(Action execute, Func<bool> can)
        {
            _execute = execute;
            _can = can;
        }
        public ActionCommand(Action execute, Func<object,bool> can)
        {
            _execute = execute;
            _canParam = can;
        }

        public ActionCommand(Action<object> execute, Func<bool> can)
        {
            _executeParam = execute;
            _can = can;
        }
        public ActionCommand(Action<object> execute, Func<object, bool> can)
        {
            _executeParam = execute;
            _canParam = can;
        }
        #endregion

        public void NotifyCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            bool rv = true;
            if (_canParam != null)
                rv = _canParam(parameter);
            if (_can != null)
                rv = _can();
            return rv;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (_executeParam != null)
                _executeParam(parameter);
            if (_execute != null)
                _execute();
        }
    }
}
