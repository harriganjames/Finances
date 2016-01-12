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
using Finances.Core;
using Finances.Core.Wpf;

namespace Finances.WinClient.Views
{
    /// <summary>
    /// Interaction logic for BankAccountsViewModel.xaml
    /// </summary>
    public partial class BankAccountsView : UserControl
    {
        public BankAccountsView()
        {
            InitializeComponent();


            this.Unloaded += (s, e) =>
            {
                //XamlUtils.DetachTriggers(s);
                int x = 1;
            };
            this.Loaded += (s, e) =>
            {
                //XamlUtils.AttachTriggers(s);
                int x = 1;
            };

            this.Initialized += (s, e) =>
            {
                int x = 1;
            };

        }
    }
}
