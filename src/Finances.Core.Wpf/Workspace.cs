using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Finances.Core.Wpf
{
    public interface IWorkspace : IViewModelBase
    {
        ICommand CloseCommand { get; }
        ICommand OpenCommand { get; }
        void WorkspaceOpened();
        void WorkspaceClosed();
        string Caption { get; }
        event EventHandler RequestClose;
        event EventHandler RequestOpen;
    }

    public class Workspace : ViewModelBase, IWorkspace
    {

        public Workspace()
        {
            this.CloseCommand = base.AddNewCommand(new ActionCommand(() =>
            {
                if (RequestClose != null) RequestClose(this, EventArgs.Empty);
            }));

            this.OpenCommand = base.AddNewCommand(new ActionCommand(() =>
            {
                if (RequestOpen != null) RequestOpen(this, EventArgs.Empty);
            }));


        }

        #region IWorkspace

        public ICommand CloseCommand { get; set; }
        public ICommand OpenCommand { get; set; }



        public virtual void WorkspaceOpened()
        {
        }

        public virtual void WorkspaceClosed()
        {
        }

        public virtual string Caption
        {
            get
            {
                return this.GetType().Name.Replace("ViewModel", "");
            }
        }

        public event EventHandler RequestClose;
        public event EventHandler RequestOpen;

        #endregion


    }
}
