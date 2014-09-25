using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Finances.Core.Wpf.Converters
{
    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || (decimal)value == 0M)
                return String.Empty;

            return String.Format("{0:n2}", ((decimal)value)*100);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal decValue = 0;
            if (value == null)
                return 0M;

            string stringValue = ((string)value).Trim();

            if (stringValue.EndsWith("%"))
                stringValue = stringValue.Remove(stringValue.Length - 1);

            if (decimal.TryParse(stringValue, out decValue))
            {
                return decValue/100;
            }
            
            return 0.0M;
        }
    }
}
