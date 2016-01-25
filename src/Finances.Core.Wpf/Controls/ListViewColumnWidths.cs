using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Finances.Core.Wpf.Controls
{
    /// <summary>
    /// Columns widths are stored in the ColumnWidths property.
    /// Bind this property to a VM property to remember columns widths.
    /// e.g.
    /// ColumnWidthStore="{Binding ViewData[ListViewColumnWidths], Mode=TwoWay}"
    /// </summary>
    public class ListViewColumnWidths : System.Windows.Controls.ListView
    {
        DependencyPropertyDescriptor dpdColumnWidth = DependencyPropertyDescriptor.FromProperty(GridViewColumn.WidthProperty, typeof(GridViewColumn));

        public ListViewColumnWidths() : base()
        {
            Console.WriteLine("ListViewColumnWidths start");

            if (!DesignerProperties.GetIsInDesignMode(this))
            {

                this.Loaded += (s, e) =>
                {
                    var v = this.View as GridView;
                    var lst = ColumnWidthStore as List<double>;

                    if (v != null)
                    {
                        int i = 0;
                        foreach (var col in v.Columns)
                        {
                            if (lst != null && i < lst.Count)
                            {
                                if (Double.IsNaN(lst[i]) && Double.IsNaN(col.Width))
                                    col.Width = 1;
                                col.Width = lst[i];
                            }
                            else
                            {
                                col.Width = 1;
                                col.Width = Double.NaN;
                            }
                            dpdColumnWidth.AddValueChanged(col, ColumnWidthChanged);
                            i++;
                        }
                    }

                };


                this.Unloaded += (s, e) =>
                {
                    var v = this.View as GridView;

                    if (v != null)
                    {
                        foreach (var col in v.Columns)
                        {
                            dpdColumnWidth.RemoveValueChanged(col, ColumnWidthChanged);
                        }
                    }

                };
                Console.WriteLine("ListViewColumnWidths start");

            }
        }

        void ColumnWidthChanged(object sender, EventArgs e)
        {
            StoreColumnWidths();
        }

        void StoreColumnWidths()
        {
            var v = this.View as GridView;

            if (v != null)
            {
                if (ColumnWidthStore == null)
                {
                    ColumnWidthStore = new List<double>();
                }

                var lst = ColumnWidthStore as List<double>;

                if (lst != null)
                {
                    int i = 0;
                    foreach (var col in v.Columns)
                    {
                        if (i >= lst.Count)
                        {
                            lst.Add(col.Width); // cannot use ActualWidth because it has not yet been updated at this point in time
                        }
                        else
                        {
                            lst[i] = col.Width; // same as above
                        }
                        i++;
                    }
                }

            }


        }



        public object ColumnWidthStore
        {
            get { return (object)GetValue(ColumnWidthStoreProperty); }
            set { SetValue(ColumnWidthStoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColumnWidths.  
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnWidthStoreProperty =
            DependencyProperty.Register("ColumnWidthStore", typeof(object), typeof(ListViewColumnWidths), new PropertyMetadata(null));


    }
}
