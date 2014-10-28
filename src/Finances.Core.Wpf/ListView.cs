namespace Finances.Core.Wpf
{
    public class ListView : System.Windows.Controls.ListView
    {
        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            if(e.ClickCount!=2)
                base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseDoubleClick(System.Windows.Input.MouseButtonEventArgs e)
        {
            //base.OnMouseDoubleClick(e);
            int i = 1;
            e.Handled = false;
        }
    }


}
