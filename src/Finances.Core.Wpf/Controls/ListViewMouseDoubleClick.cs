using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Finances.Core.Wpf.Controls
{
    public class ListViewMouseDoubleClick : ListViewColumnWidths
    {
        public ListViewMouseDoubleClick() : base()
        {
            Console.WriteLine("ListViewMouseDoubleClick start");
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                //                this.MouseDoubleClick += ListViewScroll_MouseDoubleClick;
                // now using ListViewDoubleClickCommandBahavior.cs instead
            }
            Console.WriteLine("ListViewMouseDoubleClick start");

        }

        private void ListViewScroll_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TryFindParent<GridViewColumnHeader>(e.OriginalSource as DependencyObject) == null)
            {
                var cmd = this.MouseDoubleClickCommand;
                if (cmd != null)
                {
                    if (cmd.CanExecute(this.SelectedItem))
                        cmd.Execute(this.SelectedItem);
                }
            }
        }

        private T TryFindParent<T>(DependencyObject current) where T : class
        {
            DependencyObject parent = VisualTreeHelper.GetParent(current);

            if (parent == null) return null;

            if (parent is T) return parent as T;
            else return TryFindParent<T>(parent);
        }



        public ICommand MouseDoubleClickCommand
        {
            get { return (ICommand)GetValue(MouseDoubleClickCommandProperty); }
            set { SetValue(MouseDoubleClickCommandProperty, value); }
        }

        public static readonly DependencyProperty MouseDoubleClickCommandProperty =
            DependencyProperty.Register("MouseDoubleClickCommand", typeof(ICommand), typeof(ListViewMouseDoubleClick), new PropertyMetadata(null));

    }
}
