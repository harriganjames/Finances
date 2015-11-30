using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;


/*
 * Allows the binding of an element which are not in the visual tree (and therefore don't haev inheritance context and cannot be databound)
 * 
 * Example of use:

xmlns:libctl="clr-namespace:Finances.Core.Wpf.Controls;assembly=Finances.Core.Wpf"

    <UserControl.Resources>
        <libctl:DataContextSpy x:Key="mainvm"/>
    </UserControl.Resources>

<Label Content="{Binding Source={StaticResource mainvm}, Path=DataContext.ColumnSortName[Name]}"/>

 * 
 * 
 * 
 * */


namespace Finances.Core.Wpf.Controls
{
    public class DataContextSpy
        : Freezable // Enable ElementName and DataContext bindings
    {
        public DataContextSpy()
        {
            // This binding allows the spy to inherit a DataContext.
            BindingOperations.SetBinding(this, DataContextProperty, new Binding());
        }

        public object DataContext
        {
            get { return (object)GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        // Borrow the DataContext dependency property from FrameworkElement.
        public static readonly DependencyProperty DataContextProperty =
            FrameworkElement.DataContextProperty.AddOwner(typeof(DataContextSpy));

        protected override Freezable CreateInstanceCore()
        {
            // We are required to override this abstract method.
            throw new NotImplementedException();
        }
    }
}
