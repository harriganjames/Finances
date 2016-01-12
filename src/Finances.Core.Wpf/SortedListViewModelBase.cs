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


    public interface ISortedListViewModelBase<T> : IListViewModelBase<T> where T : IItemViewModelBase
    {
        SortedListViewModelBase<T>.ColumnHeaderSortIndicator ColumnHeaderSuffix { get; }
    }

    public abstract class SortedListViewModelBase<T> : ListViewModelBase<T>, ISortedListViewModelBase<T> where T : IItemViewModelBase
    {
        CollectionViewSource dataListView;

        public SortedListViewModelBase()
        {
            this.dataListView = new CollectionViewSource();
            this.dataListView.Source = base.dataList;

            SortColumnCommand = base.AddNewCommand(new ActionCommand(SortColumn));
        }


        public ActionCommand SortColumnCommand { get; set; }



        public ListCollectionView DataListView
        {
            get
            {
                return (ListCollectionView)this.dataListView.View;
            }
        }


        ColumnHeaderSortIndicator columnHeaderSuffix;
        public ColumnHeaderSortIndicator ColumnHeaderSuffix
        {
            get
            {
                if (columnHeaderSuffix == null)
                    columnHeaderSuffix = new ColumnHeaderSortIndicator(this.dataListView);

                return columnHeaderSuffix;
            }
        }

        void SortColumn(object param)
        {
            string propertyName = param as string;
            ListSortDirection direction;
            SortDescription existingSortDescription;

            existingSortDescription = this.dataListView.SortDescriptions.FirstOrDefault(sd => sd.PropertyName == propertyName);
            if (existingSortDescription != null && existingSortDescription.PropertyName != null)
                direction = existingSortDescription.Direction == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            else
                direction = ListSortDirection.Ascending;

            this.dataListView.SortDescriptions.Clear();
            this.dataListView.SortDescriptions.Add(new SortDescription(propertyName, direction));

            NotifyPropertyChanged(() => this.ColumnHeaderSuffix);
        }


        public class ColumnHeaderSortIndicator
        {
            CollectionViewSource cvs;
            public ColumnHeaderSortIndicator(CollectionViewSource cvs)
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
