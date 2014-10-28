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
//using Finances.BankModule.ViewModels;

namespace Finances.WinClient.Views
{
    public partial class BanksView : UserControl
    {

        public BanksView()
        {
            Apex.ObjectInformation.AddObjectReference(this);

            InitializeComponent();

            this.Unloaded += (s, e) =>
            {
                XamlUtils.DetachTriggers(s);
            };

            this.Loaded += (s, e) =>
            {
                XamlUtils.AttachTriggers(s);
            };


        }

    }
}
