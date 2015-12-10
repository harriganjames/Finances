using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Finances.WinClient.DesignTimeData
{
    public class Bank //: DependencyObject // do we need this?
    {

        public string Name { get; set; }

        public BitmapSource Logo{ get; set; }

    }
}
