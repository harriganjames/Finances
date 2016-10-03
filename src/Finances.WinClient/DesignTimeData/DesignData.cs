using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Finances.WinClient.DesignTimeData
{
    public static class DesignData
    {
        static DesignData()
        {
            DataContextInheritConverter = new DataContextInheritConverter();


            Bank = new Bank()
            {
                Name = "HABS"
                //Logo = new BitmapImage(new Uri(@"/Images/DesignTime/Bank.png",UriKind.RelativeOrAbsolute))
            };
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(@"C:\Secure\Projects\DevProjects\Finances\src\Finances.WinClient\Images\DesignTime\Bank.png", UriKind.Absolute);
            bi.EndInit();
            Bank.Logo = bi;

            BankAccount = new BankAccount()
            {
                AccountName = "Current Account",
                Bank = Bank,
                SortCode = "12-34-56"
            };

            BankAccount2 = new BankAccount()
            {
                AccountName = "Savings",
                Bank = Bank,
                SortCode = "00-00-00"
            };

            BalanceDate = new BalanceDate()
            {
                DateOfBalance = new DateTime(2016, 8, 24),
                BalanceDateBankAccounts = new List<BalanceDateBankAccount>()
                {
                    new BalanceDateBankAccount()
                    {
                        BankAccount = BankAccount,
                        BalanceAmount = 123
                    },
                    new BalanceDateBankAccount()
                    {
                        BankAccount = BankAccount2,
                        BalanceAmount = 456
                    }
                }
            };
        }


        public static Bank Bank { get; set; }
        public static BankAccount BankAccount { get; set; }
        public static BankAccount BankAccount2 { get; set; }



        public static BalanceDate BalanceDate { get; set; }

        public static DataContextInheritConverter DataContextInheritConverter { get; set; }

    }
}
