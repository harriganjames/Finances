using System;
using System.Collections.ObjectModel;
using Finances.Core.Wpf.Events;

namespace Finances.Core.Wpf
{
    public interface ITreeViewItemViewModelBase : IItemViewModelBase
    {
        bool IsExpanded { get; set; }
        ITreeViewItemViewModelBase Parent { get; set; }
        ObservableCollection<ITreeViewItemViewModelBase> Children { get; }
        event EventHandler<TreeViewItemExpandedEventArgs> TreeViewItemExpanded;
    }

    /// <summary>
    /// Base class for a TreeViewItem. Supports lazy loading of children
    /// </summary>
    public class TreeViewItemViewModelBase : ItemViewModelBase, ITreeViewItemViewModelBase
    {
        bool isExpanded;

        static readonly TreeViewItemViewModelBase DummyChild = new TreeViewItemViewModelBase();

        ObservableCollection<ITreeViewItemViewModelBase> children;
        ITreeViewItemViewModelBase parent;

        public TreeViewItemViewModelBase(bool lazyLoadChildren=true)
        {
            children = new ObservableCollection<ITreeViewItemViewModelBase>();
            if(lazyLoadChildren)
                children.Add(DummyChild);
        }

        public event EventHandler<TreeViewItemExpandedEventArgs> TreeViewItemExpanded;


        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (value != isExpanded)
                {
                    isExpanded = value;
                    NotifyPropertyChanged();
                }

                // Expand all the way up to the root.
                if (isExpanded && parent != null)
                    parent.IsExpanded = true;

                // Lazy load the child items, if necessary.
                if (this.HasDummyChild)
                {
                    this.Children.Remove(DummyChild);
                }
                OnTreeViewItemExpanded();
            }
        }


        public ITreeViewItemViewModelBase Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                this.parent = value;
            }
        }

        /// <summary>
        /// Returns the logical child items of this object.
        /// </summary>
        public ObservableCollection<ITreeViewItemViewModelBase> Children
        {
            get 
            {
                return children; 
            }
        }


        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        protected virtual void LoadChildren()
        {
        }

        private void OnTreeViewItemExpanded()
        {
            if(TreeViewItemExpanded!=null)
            {
                TreeViewItemExpanded(this, new TreeViewItemExpandedEventArgs() { TreeViewItemExpanded = this });
            }
        }


        private bool HasDummyChild
        {
            get { return this.Children.Count == 1 && this.Children[0] == DummyChild; }
        }


    }
}
