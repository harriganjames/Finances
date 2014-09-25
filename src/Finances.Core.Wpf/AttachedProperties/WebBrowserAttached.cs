using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Aub.Library.AttachedProperties
{
    public class WebBrowserAttached
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.RegisterAttached(
            "Source"
            , typeof(Uri)
            , typeof(WebBrowserAttached)
            , new PropertyMetadata(null, OnSourceChanged));


        public static Uri GetSource(FrameworkElement control)
        {
            return (Uri)control.GetValue(SourceProperty);
        }

        public static void SetSource(FrameworkElement control, Uri value)
        {
            control.SetValue(SourceProperty, value);
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Uri uri = e.NewValue as Uri;

            if (e.NewValue!=null && uri == null)
                throw new InvalidOperationException(String.Format("Uri could not be resolved from e.NewValue"));

            WebBrowser wb = d as WebBrowser;

            if (wb == null)
                throw new InvalidOperationException(String.Format("WebBrowser could not be resolved from d"));

            if(uri!=null)
                wb.Source = uri;

        }


    }
}
