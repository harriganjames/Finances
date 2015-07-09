using System.ComponentModel;
using System.Windows.Controls;

namespace Finances.Core.Wpf.Controls
{
    /// <summary>
    /// New list items (or selected item) are brought into view
    /// </summary>
    public class ListViewScroll : System.Windows.Controls.ListView
    {
        public ListViewScroll()
            : base()
        {
            //if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                SelectionChanged += ListViewScroll_SelectionChanged;

                // ListView needs to be non-virtualizing for off-screen items to be brought into view
                SetValue(VirtualizingPanel.IsVirtualizingProperty, false);
            }
        }

        void ListViewScroll_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ScrollIntoView(SelectedItem);
        }

    }
}
