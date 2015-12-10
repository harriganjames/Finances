//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Data;

//using Finances.WinClient.ViewModels;

//namespace Finances.WinClient.Converters
//{
//    public class TansferToDirectionConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            var vm = value as TransferItemViewModel;
//            string direction = string.Empty;

//            if(vm!=null)
//            {
//                if(vm.FromBankAccount==null || vm.FromBankAccount.BankAccountId == -1)
//                {
//                    direction = "Inbound";
//                }
//                else if(vm.ToBankAccount==null || vm.ToBankAccount.BankAccountId==-1)
//                {
//                    direction = "Outbound";
//                }
//            }


//            return direction;
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
