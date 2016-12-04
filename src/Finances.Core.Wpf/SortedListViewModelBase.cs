using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using Finances.Utility;

namespace Finances.Core.Wpf
{
    /*
     * Adds ListView column sorting. To use: 
     * In VM:
     * Change main list property to e.g. public ListCollectionView Banks
     *  and to return base.DataListView
     *  
     * In View:
     * Change GridViewColumnHeader:
        Remove the Header property
        DirectionName is the property to sort on
        Direction is the text that appears in the header

        <GridViewColumnHeader Command="{Binding SortColumnCommand}" 
                                CommandParameter="DirectionName">
            <Label Content="{Binding ColumnHeaderSuffix[DirectionName]}"
                ContentStringFormat="Direction {0}"/>
        </GridViewColumnHeader>

     * 
     */


    public interface ISortedListViewModelBase<T> : IListViewModelBase<T> where T : class, IItemViewModelBase
    {
        SortedListViewModelBase<T>.ColumnHeaderSortIndicator ColumnHeaderSuffix { get; }
    }

    public abstract class SortedListViewModelBase<T> : ListViewModelBase<T>, ISortedListViewModelBase<T> where T : class, IItemViewModelBase
    {
        CollectionViewSource dataListView;
        DebounceExecutor<string> filterDebounce = new DebounceExecutor<string>();

        public SortedListViewModelBase()
        {
            this.dataListView = new CollectionViewSource();
            this.dataListView.Source = base.dataList;
            this.dataListView.View.Filter = ViewFilter;

            SortColumnCommand = base.AddNewCommand(new ActionCommand(SortColumn));

            filterDebounce.Initialize(new TimeSpan(0, 0, 0, 0, 500 ), FilterExecute);
        }
        //public SortedListViewModelBase(IDebounceExecutor<string> de) : base()
        //{
        //    filterDebounce = de;
        //    filterDebounce.Initialize(new TimeSpan(0, 0, 0, 1), FilterExecute);
        //}

        public ActionCommand SortColumnCommand { get; set; }



        public ListCollectionView DataListView
        {
            get
            {
                return (ListCollectionView)this.dataListView.View;
            }
        }

        string filterExpression;
        public string FilterExpression
        {
            get
            {
                return filterExpression;
            }
            set
            {
                //filterDebounce.Execute(value);
                filterExpression = value;
                Task.Factory.StartNew(() => this.DataListView.Refresh(),System.Threading.CancellationToken.None,TaskCreationOptions.None,this.UIScheduler);

                //this.DataListView.Refresh();
            }
        }

        string filterExdeuteExpression;

        void FilterExecute(string filter)
        {
            filterExdeuteExpression = filter;
            this.DataListView.Refresh();
        }


        private bool ViewFilter(object item)
        {
            if (String.IsNullOrWhiteSpace(filterExpression))
                return true;
            if (item is T)
                return FilterItem(item as T);
            return true;
        }

        public virtual bool FilterItem(T item)
        {
            return true;
        }


        ColumnHeaderSortIndicator columnHeaderSuffix;
        public ColumnHeaderSortIndicator ColumnHeaderSuffix
        {
            get
            {
                if (columnHeaderSuffix == null)
                    columnHeaderSuffix = new ColumnHeaderSortIndicator(this.dataListView.View);

                return columnHeaderSuffix;
            }
        }

        void SortColumn(object param)
        {
            string propertyName = param as string;
            ListSortDirection direction;
            SortDescription existingSortDescription;

            var view = this.dataListView.View;

            existingSortDescription = view.SortDescriptions.FirstOrDefault(sd => sd.PropertyName == propertyName);
            if (existingSortDescription != null && existingSortDescription.PropertyName != null)
                direction = existingSortDescription.Direction == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            else
                direction = ListSortDirection.Ascending;

            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription(propertyName, direction));

            NotifyPropertyChanged(() => this.ColumnHeaderSuffix);
        }


        public class ColumnHeaderSortIndicator
        {
            ICollectionView cvs;
            public ColumnHeaderSortIndicator(ICollectionView cvs)
            {
                this.cvs = cvs;
            }

            public string this[string property]
            {
                get
                {
                    var caption = new StringBuilder();
                    SortDescription existingSortDescription;

                    existingSortDescription = cvs.SortDescriptions.FirstOrDefault(sd => sd.PropertyName == property);
                    if (existingSortDescription != null && existingSortDescription.PropertyName != null)
                        caption.AppendFormat("{0}", existingSortDescription.Direction == ListSortDirection.Ascending ? "↓" : "↑");

                    return caption.ToString();
                }
            }

        }

    }
}
