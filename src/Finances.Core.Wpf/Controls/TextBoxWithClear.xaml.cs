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

namespace Finances.Core.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for TextBoxWithClear.xaml
    /// </summary>
    public partial class TextBoxWithClear : UserControl
    {
        public TextBoxWithClear()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            TextBox.TextProperty.AddOwner(typeof(TextBoxWithClear), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true });





        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Watermark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(string), typeof(TextBoxWithClear), new PropertyMetadata("Filter..."));



        private void Clear_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ClearText();
        }

        private void ClearText()
        {
            this.TB.Text = String.Empty;
        }

        private bool IsTextClear
        {
            get
            {
                return this.TB.Text == String.Empty;
            }
        }

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && !IsTextClear)
            {
                ClearText();
                e.Handled = true;
            }
        }


    }
}
