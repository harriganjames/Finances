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
    public class BankItemViewModel : TreeViewItemViewModelBase
    {
        Bank entity;

        public BankItemViewModel(Bank entity)
        {
            this.entity = entity;
        }

        #region PublicProperties

        public Bank Entity
        {
            get { return entity; }
            set
            {
                entity = value;
                NotifyAllPropertiesChanged();
            }
        }

        public int BankId
        {
            get { return entity.BankId; }
        }


        public string Name
        {
            get { return entity.Name; }
        }

        public byte[] LogoRaw
        {
            get { return entity.Logo; }
        }

        public BitmapSource Logo
        {
            get
            {
                if (entity.Logo == null) return null;
                MemoryStream mem2 = new MemoryStream(entity.Logo);
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
