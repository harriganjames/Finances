using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Finances.WinClient.ViewModels;

namespace Finances.WinClient.Converters
{
    public class AccountElsewhereToVisibileConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var account = value as BankAccountItemViewModel;

            if (!(value is BankAccountItemViewModel)) return Visibility.Collapsed;  // for design mode

            return (account == null || account.BankAccountId == -1) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
