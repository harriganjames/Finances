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

namespace Finances.WinClient.Views
{
    public partial class BanksView : UserControl
    {

        public BanksView()
        {
            InitializeComponent();


        }

        private void debug_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var x = sender;

        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var x = sender;

        }


    }
}
