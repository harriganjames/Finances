using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.sp.Children)
            {
                string s;
                FrameworkElement element = item as FrameworkElement;
                if (element != null)
                {
                    s = String.Format("Type={0}, HA={1}, VA={2}, H={3}, W={4}, MinH={5}, MinW={6}, MaxH={7}, MaxW={8}",
                        element.GetType().Name,
                        element.HorizontalAlignment,
                        element.VerticalAlignment,
                        element.Height,
                        element.Width,
                        element.MinHeight,
                        element.MinWidth,
                        element.MaxHeight,
                        element.MaxWidth
                        );
                    this.list.Items.Add(s);
                }
                else
                {
                    s = String.Format("Type={0} not a FrameworkElement", item.GetType().Name);
                }
            }

        }
    }
}
