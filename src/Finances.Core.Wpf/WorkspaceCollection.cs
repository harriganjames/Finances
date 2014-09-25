using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Finances.Core.Wpf;

namespace Finances.Core.Wpf
{
    public interface IWorkspaceCollection
    {
        void Add(string key, IWorkspace ws);
        IWorkspace Get(string key);
        IEnumerator<IWorkspace> GetEnumerator();
    }

    /// <summary>
    /// Holds the available workspaces
    /// </summary>
    public class WorkspaceCollection : IWorkspaceCollection, IEnumerable<IWorkspace>
    {
        readonly Dictionary<string, IWorkspace> workspaces = new Dictionary<string, IWorkspace>();

        public WorkspaceCollection(IEnumerable<IWorkspace> workspaces)
        {
            foreach (IWorkspace ws in workspaces)
            {
                this.workspaces.Add(ws.Caption, ws);
            }
        
        }

        public void Add(string key, IWorkspace ws)
        {
            workspaces.Add(key, ws);
        }

        public IWorkspace Get(string key)
        {
            return workspaces[key];
        }



        public IEnumerator<IWorkspace> GetEnumerator()
        {
            return workspaces.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return workspaces.Values.GetEnumerator();
        }
    }
}
