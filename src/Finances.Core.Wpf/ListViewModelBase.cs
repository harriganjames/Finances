using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.Wpf
{
    public interface IListViewModelBase<T> : IWorkspace where T : IItemViewModelBase
    {
        ObservableCollection<T> DataList { get; }
        int GetQtySelected();
        IEnumerable<T> GetSelectedItems();
    }

    public abstract class ListViewModelBase<T> : Workspace, IListViewModelBase<T> where T : IItemViewModelBase
    {
        ObservableCollection<T> _dataList = new ObservableCollection<T>();

        public ListViewModelBase()
        {

            _dataList.CollectionChanged += _dataList_CollectionChanged;
        }

        // Manage the SelectedChanged events for the items in the list
        void _dataList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (ItemViewModelBase vm in e.NewItems)
                    vm.SelectedChanged += item_SelectedChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (ItemViewModelBase vm in e.OldItems)
                    vm.SelectedChanged -= item_SelectedChanged;
        }

        void item_SelectedChanged(object sender, BooleanResultEventArgs e)
        {
            this.ItemSelectionChanged();
        }

        protected virtual void ItemSelectionChanged()
        {
            base.RefreshCommands();
        }



        public ObservableCollection<T> DataList
        {
            get
            {
                return _dataList;
            }
        }


        public int GetQtySelected()
        {
            return _dataList.Count(b => b.IsSelected);
        }
        public IEnumerable<T> GetSelectedItems()
        {
            return _dataList.Where(b => b.IsSelected);
        }

        protected void ItemSelectionChangedHandler(object o, BooleanResultEventArgs e)
        {
            base.RefreshCommands();
        }


    }
}
