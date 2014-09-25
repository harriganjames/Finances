using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Data;

namespace Finances.Core.Wpf
{
    public interface IWorkspaceManager
    {
        void CloseWorkspace(IWorkspace ws);
        void CloseAllWorkspaces();
        void OpenWorkspace(string name);
        void OpenWorkspace(IWorkspace ws);
        ObservableCollection<IWorkspace> OpenWorkspaces { get; }
        ObservableCollection<IWorkspace> RegisteredWorkspaces { get; }
        void AddWorkspace(IWorkspace ws);
        IWorkspace ActiveWorkspace { get; set; }
        event EventHandler ActiveWorkspaceChanged;
    }

    /// <summary>
    /// Manages active workspaces
    /// </summary>
    public class WorkspaceManager : IWorkspaceManager
    {
        ObservableCollection<IWorkspace> openWorkspaces = new ObservableCollection<IWorkspace>();
        ObservableCollection<IWorkspace> registereWorkspaces = new ObservableCollection<IWorkspace>();
        IWorkspace activeWorkspace;

        readonly IWorkspaceCollection registeredWorkspaceCollection;

        public WorkspaceManager(IWorkspaceCollection workspaces)
        {
            registeredWorkspaceCollection = workspaces;

            openWorkspaces.CollectionChanged += workspaces_CollectionChanged;


            foreach (var ws in registeredWorkspaceCollection)
            {
                this.registereWorkspaces.Add(ws);
                ws.RequestOpen += this.workspace_RequestOpen;
            }
        
        }

        public event EventHandler ActiveWorkspaceChanged;

        public ObservableCollection<IWorkspace> RegisteredWorkspaces
        {
            get { return this.registereWorkspaces; }
        }

        public ObservableCollection<IWorkspace> OpenWorkspaces
        {
            get { return openWorkspaces; }
        }


        public IWorkspace ActiveWorkspace
        {
            get
            {
                return this.activeWorkspace;
            }
            set
            {
                if (this.OpenWorkspaces.Contains(value) || value==null)
                    this.activeWorkspace = value;
            }
        }


        public void AddWorkspace(IWorkspace ws)
        {
            this.OpenWorkspaces.Add(ws);
            this.SetActiveWorkspace(ws);
            ws.WorkspaceOpened();
        }


        public void OpenWorkspace(string name)
        {
            IWorkspace ws;
            ws = registeredWorkspaceCollection.Get(name);
            OpenWorkspace(ws);
        }

        public void OpenWorkspace(IWorkspace ws)
        {
            if (!this.OpenWorkspaces.Contains(ws))
            {
                this.OpenWorkspaces.Add(ws);
                ws.WorkspaceOpened();
            }

            this.SetActiveWorkspace(ws);
        }

        public void CloseWorkspace(IWorkspace ws)
        {
            if (openWorkspaces.Contains(ws))
            {
                openWorkspaces.Remove(ws);
                ws.WorkspaceClosed();
            }
        }

        public void CloseAllWorkspaces()
        {
            foreach (var item in openWorkspaces.ToList<IWorkspace>())
            {
                this.CloseWorkspace(item);
            }
        }


        void SetActiveWorkspace(IWorkspace ws)
        {
            if (this.activeWorkspace != ws)
            {
                this.activeWorkspace = ws;
                if (this.ActiveWorkspaceChanged != null)
                    this.ActiveWorkspaceChanged(this, new EventArgs());
            }
        }


        void workspaces_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (IWorkspace vm in e.NewItems)
                {
                    vm.RequestClose += this.workspace_RequestClose;
                    //vm.RequestOpen += this.workspace_RequestOpen;
                }

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (IWorkspace vm in e.OldItems)
                {
                    vm.RequestClose -= this.workspace_RequestClose;
                    //vm.RequestOpen -= this.workspace_RequestOpen;
                }
        }


        void workspace_RequestClose(object sender, EventArgs e)
        {
            IWorkspace ws = sender as IWorkspace;
            if (ws != null)
            {
                this.CloseWorkspace(ws);
            }
        }

        void workspace_RequestOpen(object sender, EventArgs e)
        {
            IWorkspace ws = sender as IWorkspace;
            if (ws != null)
            {
                this.OpenWorkspace(ws);
            }
        }


    }
}
