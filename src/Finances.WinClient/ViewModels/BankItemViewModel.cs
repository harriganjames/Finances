using System.IO;
using System.Windows.Media.Imaging;
using Finances.Core.Wpf;
using Finances.WinClient.DomainServices;


namespace Finances.WinClient.ViewModels
{
    public interface IBankItemViewModel : IItemViewModelBase
    {
        int BankId { get; set; }
        string Name { get; set; }
        byte[] LogoRaw { get; set; }
        //byte[] Logo { get; }
        BitmapSource Logo { get; }
    }

    public class BankItemViewModel : ItemViewModelBase, IBankItemViewModel
    {
        int bankId;
        string name;
        byte[] logo;

        #region PublicProperties

        public int BankId
        {
            get { return this.bankId; }
            set
            {
                this.bankId = value;
                NotifyPropertyChanged();
            }
        }


        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                NotifyPropertyChanged();
            }
        }

        public byte[] LogoRaw
        {
            get { return this.logo; }
            set
            {
                this.logo = value;
                NotifyPropertyChanged(() => this.Logo);
            }
        }


        //public byte[] Logo
        //{
        //    get { return this.logo; }
        //}

        public BitmapSource Logo
        {
            get
            {
                if (this.logo == null) return null;
                MemoryStream mem2 = new MemoryStream(this.logo);
                BmpBitmapDecoder dec = new BmpBitmapDecoder(mem2, BitmapCreateOptions.None, BitmapCacheOption.Default);
                return dec.Frames[0];
            }
        }


        #endregion

        public override string ToString()
        {
            return this.Name;
        }

    }
}
