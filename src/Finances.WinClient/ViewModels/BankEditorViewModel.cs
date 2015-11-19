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
using Finances.Interface;
using Finances.WinClient.DomainServices;


namespace Finances.WinClient.ViewModels
{
    public class BankEditorViewModel : EditorViewModelBase
    {
        bool delayValidation = false; // must be false until change logic around IsValid
        List<DataIdName> existingBanks;
        Bank entity;

        readonly IBankRepository bankRepository;
        readonly IDialogService dialogService;

        public BankEditorViewModel(
                        IBankRepository bankRepository,
                        IDialogService dialogService,
                        Bank entity)
        {
            this.bankRepository = bankRepository;
            this.dialogService = dialogService;
            this.entity = entity;

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


        public int BankId
        {
            get { return entity.BankId; }
            set
            {
                if (entity.BankId != value)
                {
                    entity.BankId = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [Required(ErrorMessage = "Bank Name is mandatory")]
        public string Name
        {
            get { return entity.Name; }
            set
            {
                if (entity.Name != value)
                {
                    entity.Name = value;
                    NotifyPropertyChangedAndValidate();
                }
            }
        }

        public byte[] LogoRaw
        {
            get { return entity.Logo; }
            set 
            {
                entity.Logo = value;
                NotifyPropertyChanged(()=>this.Logo);
            }
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
            existingBanks = bankRepository.ReadListDataIdName();
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
                if (existingBanks.Exists(n => n.Name.Equals(this.Name,StringComparison.CurrentCultureIgnoreCase) && n.Id!=BankId))
                {
                    base.ValidationHelper.AddValidationMessage("Bank Name already exists", "Name");
                }
            }

        }

        #endregion


    }
}
