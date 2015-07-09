using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Finances.Core.Entities;
using Finances.WinClient.ViewModels;

namespace Finances.WinClient.Converters
{
    public class BalanceStateToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var brush = new SolidColorBrush();
            var balanceState = (BalanceStateEnum)value;

            switch (balanceState)
            {
                case BalanceStateEnum.OK:
                    brush.Color = Color.FromRgb(0, 200, 0);
                    break;
                case BalanceStateEnum.BelowThreshold:
                    brush.Color = Color.FromRgb(255, 150, 0);
                    break;
                case BalanceStateEnum.Negative:
                    brush.Color = Color.FromRgb(255, 0, 0);
                    break;
                default:
                    brush.Color = Color.FromRgb(0, 0, 0);
                    break;
            }

            return brush;
        }



        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
