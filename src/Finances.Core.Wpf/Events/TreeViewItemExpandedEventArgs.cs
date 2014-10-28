using System;

namespace Finances.Core.Wpf.Events
{
    public class TreeViewItemExpandedEventArgs : EventArgs
    {
        public TreeViewItemViewModelBase TreeViewItemExpanded { get; set; }
    }
}
