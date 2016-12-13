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
        public Bank()
        {
            Name = "HABS2";
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(@"C:\Secure\Projects\DevProjects\Finances\src\Finances.WinClient\Images\DesignTime\Bank.png", UriKind.Absolute);
            bi.EndInit();
            Logo = bi;

        }

        public string Name { get; set; }
        public bool HasLogo
        {
            get
            {
                return Logo != null;
            }
        }
        public BitmapSource Logo{ get; set; }

    }
}
