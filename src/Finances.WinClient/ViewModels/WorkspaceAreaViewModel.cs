using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Finances.Core.Wpf;

namespace Finances.WinClient.ViewModels
{
    public interface IWorkspaceAreaViewModel
    {
        ActionCommand OpenWorkspaceCommand { get; set; }
        IWorkspace SelectedWorkspace { get; set; }
        void CloseAllWorkspaces();
        ObservableCollection<IWorkspace> OpenWorkspaces { get; }
        ObservableCollection<IWorkspace> RegisteredWorkspaces { get; }
    }

    public class WorkspaceAreaViewModel : ViewModelBase, IWorkspaceAreaViewModel
    {
        readonly IWorkspaceManager workspaceManager;

        public WorkspaceAreaViewModel(IWorkspaceManager workspaceManager)
        {
            this.workspaceManager = workspaceManager;

            OpenWorkspaceCommand = new ActionCommand(OpenWorkspace);

            this.workspaceManager.ActiveWorkspaceChanged += (s, e) =>
            {
                NotifyPropertyChanged(() => this.SelectedWorkspace);
            };

        }

        public ActionCommand OpenWorkspaceCommand { get; set; }

        public ObservableCollection<IWorkspace> OpenWorkspaces
        {
            get 
            { 
                return workspaceManager.OpenWorkspaces; 
            }
        }

        public ObservableCollection<IWorkspace> RegisteredWorkspaces
        {
            get
            {
                return workspaceManager.RegisteredWorkspaces;
            }
        }


        public IWorkspace SelectedWorkspace
        {
            get
            {
                return workspaceManager.ActiveWorkspace;
            }
            set
            {
                workspaceManager.ActiveWorkspace = value;
                NotifyPropertyChanged();
            }
        }

        public void CloseAllWorkspaces()
        {
            this.workspaceManager.CloseAllWorkspaces();
        }


        #region Privates

        private void OpenWorkspace(object name)
        {
            workspaceManager.OpenWorkspace((string)name);
        }

        #endregion

    }
}
