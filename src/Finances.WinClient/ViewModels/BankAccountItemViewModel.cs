using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Core.Wpf;

namespace Finances.WinClient.ViewModels
{
    //public interface BankAccountItemViewModel : ITreeViewItemViewModelBase, IEntityMapper<BankAccount>
    //{
    //    ICommand GoOnlineCommand { get; set; }

    //    int BankAccountId { get; set; }
    //    string AccountName { get; set; }
    //    string AccountNumber { get; set; }
    //    string AccountOwner { get; set; }
    //    BankItemViewModel Bank { get; set; }
    //    DateTime? ClosedDate { get; set; }
    //    decimal? InitialRate { get; set; }
    //    string LoginId { get; set; }
    //    string LoginUrl { get; set; }
    //    DateTime? MilestoneDate { get; set; }
    //    string MilestoneNotes { get; set; }
    //    string Notes { get; set; }
    //    DateTime OpenedDate { get; set; }
    //    string PasswordHint { get; set; }
    //    bool PaysTaxableInterest { get; set; }
    //    string SortCode { get; set; }
    //    //BankAccountItemViewModel MapIn(BankAccount from);
    //}

    public class BankAccountItemViewModel : TreeViewItemViewModelBase//, BankAccountItemViewModel
    {
        //readonly IEventPublisher eventPublisher;

        public BankAccountItemViewModel()
        {

            //this.eventPublisher = eventPublisher;

            GoOnlineCommand = base.AddNewCommand(new ActionCommand(GoOnline));
        }


        #region Static

        static BankAccountItemViewModel elsewhere;
        public static BankAccountItemViewModel Elsewhere 
        {
            get
            {
                if(elsewhere==null)
                    elsewhere = new BankAccountItemViewModel() { BankAccountId = -1, AccountName = "<elsewhere>" };
                return elsewhere;
            }
        }


        #endregion

        int bankAccountId;
        public int BankAccountId
        {
            get { return bankAccountId; }
            set { bankAccountId = value; NotifyPropertyChanged(); }
        }

        string accountName;
        public string AccountName
        {
            get { return accountName; }
            set {
                accountName = value; 
                
                NotifyPropertyChanged();
            }
        }

        //int bankId;
        //public int BankId
        //{
        //    get { return bankId; }
        //    set { bankId = value; NotifyPropertyChanged(); }
        //}

        BankItemViewModel bank;
        public BankItemViewModel Bank
        {
            get 
            {
                if (bank == null)
                    bank = new BankItemViewModel();
                return bank; 
            }
            set { bank = value; NotifyPropertyChanged(); }
        }

        string sortCode;
        public string SortCode
        {
            get { return sortCode; }
            set { sortCode = value; NotifyPropertyChanged(); }
        }

        string accountNumber;
        public string AccountNumber
        {
            get { return accountNumber; }
            set { accountNumber = value; NotifyPropertyChanged(); }
        }

        string accountOwner;
        public string AccountOwner
        {
            get { return accountOwner; }
            set { accountOwner = value; NotifyPropertyChanged(); }
        }

        bool paysTaxableInterest;
        public bool PaysTaxableInterest
        {
            get { return paysTaxableInterest; }
            set { paysTaxableInterest = value; NotifyPropertyChanged(); }
        }

        string loginUrl;
        public string LoginUrl
        {
            get { return loginUrl; }
            set { loginUrl = value; NotifyPropertyChanged(); }
        }

        string loginId;
        public string LoginId
        {
            get { return loginId; }
            set { loginId = value; NotifyPropertyChanged(); }
        }

        string passwordHint;
        public string PasswordHint
        {
            get { return passwordHint; }
            set { passwordHint = value; NotifyPropertyChanged(); }
        }

        DateTime openedDate;
        public DateTime OpenedDate
        {
            get { return openedDate; }
            set { openedDate = value; NotifyPropertyChanged(); }
        }

        DateTime? closedDate;
        public DateTime? ClosedDate
        {
            get { return closedDate; }
            set { closedDate = value; NotifyPropertyChanged(); }
        }

        decimal? initialRate;
        public decimal? InitialRate
        {
            get { return initialRate; }
            set { initialRate = value; NotifyPropertyChanged(); }
        }

        DateTime? milestoneDate;
        public DateTime? MilestoneDate
        {
            get { return milestoneDate; }
            set { milestoneDate = value; NotifyPropertyChanged(); }
        }

        string milestoneNotes;
        public string MilestoneNotes
        {
            get { return milestoneNotes; }
            set { milestoneNotes = value; NotifyPropertyChanged(); }
        }

        string notes;
        public string Notes
        {
            get { return notes; }
            set { notes = value; NotifyPropertyChanged(); }
        }


        public ICommand GoOnlineCommand { get; set; }

        private void GoOnline()
        {
            //OpenBrowserEvent ev = new OpenBrowserEvent();
            //ev.Url = this.LoginUrl;

            //this.eventPublisher.SendMessage<OpenBrowserEvent>(ev);
        }

        //#region IEntityMapper

        //public void MapIn(BankAccount entity)
        //{
        //    this.BankAccountId = entity.BankAccountId;
        //    this.AccountName = entity.Name;
        //    this.Bank.MapIn(entity.Bank);
        //    this.AccountNumber = entity.AccountNumber;
        //    this.SortCode = entity.SortCode;
        //    this.AccountOwner = entity.AccountOwner;
        //    this.OpenedDate = entity.OpenedDate;
        //    this.ClosedDate = entity.ClosedDate;
        //    this.InitialRate = entity.InitialRate;
        //    this.LoginId = entity.LoginID;
        //    this.LoginUrl = entity.LoginURL;
        //    this.MilestoneDate = entity.MilestoneDate;
        //    this.MilestoneNotes = entity.MilestoneNotes;
        //    this.Notes = entity.Notes;
        //    this.PasswordHint = entity.PasswordHint;
        //    this.PaysTaxableInterest = entity.PaysTaxableInterest;
        //}

        //public void MapOut(BankAccount entity)
        //{
        //    entity.BankAccountId = this.BankAccountId;
        //}

        //#endregion

    }
}
