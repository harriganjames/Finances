using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Core.Wpf;
using Finances.WinClient.DomainServices;


namespace Finances.WinClient.ViewModels
{
    public interface IBankItemViewModel : ITreeViewItemViewModelBase, IEntityMapper<Bank>
    {
        int BankId { get; set; }
        string Name { get; set; }
        byte[] LogoRaw { get; set; }
        //byte[] Logo { get; }
        BitmapSource Logo { get; }

        //IBankItemViewModel MapIn(Bank from);
    }

    public class BankItemViewModel : TreeViewItemViewModelBase, IBankItemViewModel
    {

        public BankItemViewModel()
        {
        }

        #region PublicProperties


        int bankId;
        public int BankId
        {
            get { return this.bankId; }
            set
            {
                this.bankId = value;
                NotifyPropertyChanged();
            }
        }


        string name;
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                NotifyPropertyChanged();
            }
        }

        byte[] logo;
        public byte[] LogoRaw
        {
            get { return this.logo; }
            set
            {
                this.logo = value;
                NotifyPropertyChanged(() => this.Logo);
            }
        }

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


        //ObservableCollection<IBankAccountItemViewModel> bankAccounts = new ObservableCollection<IBankAccountItemViewModel>();
        //public ObservableCollection<IBankAccountItemViewModel> BankAccounts 
        //{
        //    get
        //    {
        //        return bankAccounts;
        //    }
        //}

        #endregion


        public void MapIn(Bank from)
        {
            this.BankId = from.BankId;
            this.Name = from.Name;
            this.LogoRaw = from.Logo;
        }

        public void MapOut(Bank entity)
        {
            entity.BankId = this.BankId;
        }


        public override string ToString()
        {
            return this.Name;
        }


    }
}
