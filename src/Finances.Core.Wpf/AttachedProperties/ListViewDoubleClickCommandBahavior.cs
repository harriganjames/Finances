using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Finances.Core.Wpf.Controls;


// This is a good basic example of an attched property/behavior.
// An alternative implementation is to put this functionality into a subclass (see ListViewMouseDoubleClick).
// An advantage of the subclassing method is that if you know you will always want the feature and it has no "parameters"
// then you don't need to set a property to enable the feature.

/// <summary>
/// Invokes a command on DoubleClick on an item in the list, but not when the columns headers are double-clicked. 
/// 
/// Usage:
/// libattach:ListViewDoubleClickBehavior.DoubleClickCommand="{Binding EditCommand}"
/// </summary>
namespace Finances.Core.Wpf.AttachedProperties
{
    public class ListViewDoubleClickBehavior
    {

        #region Dependency Properties

        /// <summary>
        /// ItemsSource property
        /// </summary>
        public static readonly DependencyProperty DoubleClickCommandProperty =
            DependencyProperty.RegisterAttached("DoubleClickCommand",
                                                typeof(ICommand),
                                                typeof(ListViewDoubleClickBehavior),
                                                new UIPropertyMetadata(null,
                                                                       OnDoubleClickCommandPropertyChanged));

        /// <summary>
        /// Sets value of DoubleClickCommand Dependency property
        /// </summary>
        public static void SetDoubleClickCommand(DependencyObject parent, ICommand source)
        {
            parent.SetValue(DoubleClickCommandProperty, source);
        }

        /// <summary>
        /// Gets value of DoubleClickCommand Dependency property
        /// </summary>
        public static ICommand GetDoubleClickCommand(DependencyObject tab)
        {
            return tab.GetValue(DoubleClickCommandProperty) as ICommand;
        }

        #endregion  


        private static void OnDoubleClickCommandPropertyChanged(DependencyObject parent, DependencyPropertyChangedEventArgs e)
        {
            ListView listView = parent as ListView;

            if (listView == null)
                return;

            listView.MouseDoubleClick += ListView_MouseDoubleClick;
        }

        static private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListView listView = sender as ListView;

            if (listView == null)
                return;

            if (TryFindParent<GridViewColumnHeader>(e.OriginalSource as DependencyObject) == null)
            {
                var cmd = GetDoubleClickCommand(listView);
                if (cmd != null)
                {
                    if (cmd.CanExecute(listView.SelectedItem))
                        cmd.Execute(listView.SelectedItem);
                }
            }
        }

        static private T TryFindParent<T>(DependencyObject current) where T : class
        {
            DependencyObject parent = VisualTreeHelper.GetParent(current);

            if (parent == null) return null;

            if (parent is T) return parent as T;
            else return TryFindParent<T>(parent);
        }


    }
}
