using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using AutoMapper;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Core.Wpf;
using Finances.WinClient.DomainServices;


namespace Finances.WinClient.ViewModels
{
    public interface IBankEditorViewModel : IEditorViewModelBase, IEntityMapper<Bank>
    {
        int BankId { get; set; }
        string Name { get; set; }
        byte[] LogoRaw { get; set; }
        BitmapSource Logo { get; }
        void InitializeForAddEdit(bool AddEdit);
        ActionCommand ImportLogoCommand { get; set; }
        ActionCommand ClearLogoCommand { get; set; }
    }

    public class BankEditorViewModel : EditorViewModelBase, IBankEditorViewModel
    {
        bool delayValidation = false; // must be false until change logic around IsValid
        List<DataIdName> existingBanks;

        readonly IBankService bankService;
        readonly IDialogService dialogService;

        public BankEditorViewModel(
                        IBankService bankService,
                        IDialogService dialogService)
        {
            this.bankService = bankService;
            this.dialogService = dialogService;

            ImportLogoCommand = base.AddNewCommand(new ActionCommand(this.ImportLogo));
            ClearLogoCommand = base.AddNewCommand(new ActionCommand(ClearLogo,CanClearLogo));
        }

        #region PublicProperties

        public ActionCommand ImportLogoCommand { get; set; }
        public ActionCommand ClearLogoCommand { get; set; }


        public string DialogTitle
        {
            get
            {
                return this.BankId == 0 ? String.Format("Add new Bank") : String.Format("Edit Bank {0}", this.Name);
            }
        }


        int _bankId;
        public int BankId
        {
            get { return _bankId; }
            set
            {
                if (_bankId != value)
                {
                    _bankId = value;
                    NotifyPropertyChanged();
                }
            }
        }

        string _name;
        [Required(ErrorMessage = "Bank Name is mandatory")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged();
                    base.Validate();
                }
            }
        }

        byte[] _logo;

        public byte[] LogoRaw
        {
            get { return _logo; }
            set 
            { 
                _logo = value;
                NotifyPropertyChanged(()=>this.Logo);
            }
        }

        public BitmapSource Logo
        {
            get
            {
                if (_logo == null) return null;
                MemoryStream mem2 = new MemoryStream(_logo);
                BmpBitmapDecoder dec = new BmpBitmapDecoder(mem2, BitmapCreateOptions.None, BitmapCacheOption.Default);
                return dec.Frames[0];
            }
        }

        #endregion


        #region PublicMethods

        public void InitializeForAddEdit(bool addMode)
        {
            base.ValidationHelper.Enabled = !delayValidation;
            base.ValidationHelper.Reset();

            LoadExistingBanks();

            if (base.ValidationHelper.Enabled)
                base.Validate();


            if (addMode)
            {
                this.BankId = 0;
                this.Name = "";
                this.LogoRaw = null;
            }

        }

        #endregion

        #region Privates

        private void ImportLogo()
        {
            string[] files = this.dialogService.ShowOpenFileDialog(filter: "Image Files|*.bmp;*.gif;*.jpg;*.png;*.img", multi: false);


            if (files.Length > 0)
            {
                // load and scale image
                BitmapImage src = new BitmapImage(new Uri(files[0]));

                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                if (src.PixelWidth / src.PixelHeight > 200D / 100D)
                    bmp.DecodePixelWidth = 200;
                else
                    bmp.DecodePixelHeight = 100;
                bmp.UriSource = src.UriSource;
                bmp.EndInit();

                // convert to byte[] in bmp format
                BmpBitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bmp));
                MemoryStream mem = new MemoryStream();
                enc.Save(mem);
                this.LogoRaw = mem.GetBuffer();
                //NotifyPropertyChanged("Logo");
                base.RefreshCommands();
            }
        }

        private bool CanClearLogo()
        {
            return !(this.Logo == null);
        }
        private void ClearLogo()
        {
            this.LogoRaw = null;
            base.RefreshCommands();
        }


        private void LoadExistingBanks()
        {
            existingBanks = bankService.ReadListDataIdName();
        }

        #endregion


        #region Map

        public void MapOut(Bank to)
        {
            to.BankId = this.BankId;
            to.Name = this.Name;
            to.Logo = this.LogoRaw;
        }

        public void MapIn(Bank from)
        {
            this.BankId = from.BankId;
            this.Name = from.Name;
            this.LogoRaw = from.Logo;
        }


        #endregion


        #region Validation

        // Use annotations and/or ValidateDate() method
        // Add errors to ValidationHelper:
        // base.ValidationHelper.AddValidationMessage("Bank Name already exists", "Name");

        protected override void ValidateData()
        {
            // Bank Name exists
            if (existingBanks != null)
            {
                if (existingBanks.Exists(n => n.Name.Equals(this.Name,StringComparison.CurrentCultureIgnoreCase) && n.Id!=_bankId))
                {
                    base.ValidationHelper.AddValidationMessage("Bank Name already exists", "Name");
                }
            }

        }

        #endregion


    }
}
