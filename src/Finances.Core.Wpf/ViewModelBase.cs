using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Finances.Interface;

namespace Finances.Core.Wpf
{
    public interface IViewModelBase : IDialog
    {
        void Dispose();
        bool IsBusy { get; set; }
        void NotifyAllPropertiesChanged();
        void NotifyPropertyChanged(System.Linq.Expressions.Expression<Func<object>> propertyExpression);
        void NotifyPropertyChanged(string propertyName = "");
        event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
//        string Title { get; }
    }

    public abstract class ViewModelBase : NotifyBase, IDisposable, IViewModelBase, IDialog
    {
        string _title;
        bool _isBusy = false;
        List<ActionCommand> _commands = new List<ActionCommand>();
        TaskScheduler uiScheduler;

        //public event PropertyChangedEventHandler PropertyChanged;


        public ViewModelBase()
        {
            this.uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }


        public TaskScheduler UIScheduler
        {
            get
            {
                return this.uiScheduler;
            }
        }

        protected ActionCommand AddNewCommand(ActionCommand c)
        {
            _commands.Add(c);
            return c;
        }

        protected List<ActionCommand> Commands
        {
            get
            {
                return _commands;
            }
        }

        protected void RefreshCommands()
        {
            _commands.ForEach(c => c.NotifyCanExecuteChanged());
        }





        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                NotifyPropertyChanged();
            }
        }


        //public virtual string Title
        //{
        //    get
        //    {
        //        if (String.IsNullOrEmpty(_title))
        //        {
        //            return this.GetType().Name;
        //        }
        //        return _title;
        //    }
        //    //set
        //    //{
        //    //    _title = value;
        //    //}
        //}



        //// The CallerMemberName attribute that is applied to the optional propertyName 
        //// parameter causes the property name of the caller to be substituted as an argument. 
        //public virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        //public virtual void NotifyPropertyChanged(Expression<Func<object>> propertyExpression)
        //{
        //    if (propertyExpression != null)
        //    {
        //        if (propertyExpression.Body is MemberExpression)
        //        {
        //            this.NotifyPropertyChanged(((MemberExpression)propertyExpression.Body).Member.Name);
        //            return;
        //        }
        //    }
        //    throw new Exception("Invalid property expression passed into NotifyPropertyChanged");
        //}

        //public virtual void NotifyAllPropertiesChanged()
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(""));
        //    }
        //}



        public virtual new void Dispose()
        {
            _commands = null;
            base.Dispose();
        }


        #region IDialog

        private ICommand dialogAcceptCommand = new RoutedCommand();

        public ICommand DialogAcceptCommand
        {
            get { return dialogAcceptCommand; }
        }

        public virtual void DialogOkClicked()
        {
        }

        #endregion



    }
}
