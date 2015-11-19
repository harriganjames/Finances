using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Finances.Core.Entities;
using Finances.Core.ValueObjects;
using Finances.WinClient.ViewModels;

namespace Finances.WinClient.Converters
{
    public class BalanceStateToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var brush = new SolidColorBrush();
            var balanceState = value as BalanceState;

            if(balanceState!=null)
                brush.Color = Color.FromRgb(balanceState.ColourRBG[0], balanceState.ColourRBG[1], balanceState.ColourRBG[2]);
            else
                brush.Color = Color.FromRgb(0, 0, 0);

            //switch (balanceState)
            //{
            //    case BalanceStateEnum.OK:
            //        brush.Color = Color.FromRgb(0, 200, 0);
            //        break;
            //    case BalanceStateEnum.BelowThreshold:
            //        brush.Color = Color.FromRgb(255, 150, 0);
            //        break;
            //    case BalanceStateEnum.Negative:
            //        brush.Color = Color.FromRgb(255, 0, 0);
            //        break;
            //    default:
            //        brush.Color = Color.FromRgb(0, 0, 0);
            //        break;
            //}

            return brush;
        }



        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
