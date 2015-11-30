using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

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
        protected ObservableCollection<T> dataList = new ObservableCollection<T>();
        //CollectionViewSource dataListView;

        public ListViewModelBase()
        {
            dataList.CollectionChanged += _dataList_CollectionChanged;

            //this.dataListView = new CollectionViewSource();
            //this.dataListView.Source = dataList;        
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
                return dataList;
            }
        }

        //public ListCollectionView DataListView
        //{
        //    get
        //    {
        //        return (ListCollectionView)this.dataListView.View;
        //    }
        //}


        public int GetQtySelected()
        {
            return dataList.Count(b => b.IsSelected);
        }
        public IEnumerable<T> GetSelectedItems()
        {
            return dataList.Where(b => b.IsSelected);
        }

        protected void ItemSelectionChangedHandler(object o, BooleanResultEventArgs e)
        {
            base.RefreshCommands();
        }


        // sorting

        //ColumnHeaderCaption columnHeader;
        //public ColumnHeaderCaption ColumnHeader
        //{
        //    get
        //    {
        //        if (columnHeader == null)
        //            columnHeader = new ColumnHeaderCaption(this.dataListView);

        //        return columnHeader;
        //    }
        //}

        //protected void SortColumn(object param)
        //{
        //    string propertyName = param as string;
        //    ListSortDirection direction;
        //    SortDescription existingSortDescription;

        //    existingSortDescription = this.dataListView.SortDescriptions.FirstOrDefault(sd => sd.PropertyName == propertyName);
        //    if (existingSortDescription != null && existingSortDescription.PropertyName != null)
        //        direction = existingSortDescription.Direction == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
        //    else
        //        direction = ListSortDirection.Ascending;

        //    this.dataListView.SortDescriptions.Clear();
        //    this.dataListView.SortDescriptions.Add(new SortDescription(propertyName, direction));

        //    NotifyPropertyChanged(() => this.ColumnHeader);
        //}


        //public class ColumnHeaderCaption
        //{
        //    CollectionViewSource cvs;
        //    public ColumnHeaderCaption(CollectionViewSource cvs)
        //    {
        //        this.cvs = cvs;
        //    }

        //    public string this[string property]
        //    {
        //        get
        //        {
        //            var caption = new StringBuilder();
        //            SortDescription existingSortDescription;

        //            caption.Append(property);

        //            existingSortDescription = cvs.SortDescriptions.FirstOrDefault(sd => sd.PropertyName == property);
        //            if (existingSortDescription != null && existingSortDescription.PropertyName != null)
        //                caption.AppendFormat(" {0}", existingSortDescription.Direction == ListSortDirection.Ascending ? "↓" : "↑");

        //            return caption.ToString();
        //        }
        //    }

        //}

    }
}
