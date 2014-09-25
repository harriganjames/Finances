using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Finances.Core.Wpf;

namespace Finances.WinClient.ViewModels
{
    //public interface ITransferItemViewModel : IItemViewModelBase
    //{

    //    int TransferId { get; set; }
    //    string Name { get; set; }
    //    BankAccountItemViewModel FromBankAccount { get; set; }
    //    BankAccountItemViewModel ToBankAccount { get; set; }
    //    decimal Amount { get; set; }
    //    decimal AmountTolerence { get; set; }
    //    System.DateTime StartDate { get; set; }
    //    Nullable<System.DateTime> EndDate { get; set; }
    //    string Frequency { get; set; }
    //    bool IsEnabled { get; set; }
    //    System.DateTime RecordCreatedDateTime { get; set; }
    //    System.DateTime RecordUpdatedDateTime { get; set; }

    //}

    //public class TransferItemViewModel : ItemViewModelBase, ITransferItemViewModel
    //{

    //    public TransferItemViewModel()
    //    {
    //    }

    //    int transferId;
    //    public int TransferId
    //    {
    //        get { return transferId; }
    //        set { transferId = value; NotifyPropertyChanged(); }
    //    }

    //    string accountName;
    //    public string AccountName
    //    {
    //        get { return accountName; }
    //        set { accountName = value; NotifyPropertyChanged(); }
    //    }

    //    //int bankId;
    //    //public int BankId
    //    //{
    //    //    get { return bankId; }
    //    //    set { bankId = value; NotifyPropertyChanged(); }
    //    //}

    //    IBankItemViewModel bank;
    //    public IBankItemViewModel Bank
    //    {
    //        get { return bank; }
    //        set { bank = value; NotifyPropertyChanged(); }
    //    }

    //    string sortCode;
    //    public string SortCode
    //    {
    //        get { return sortCode; }
    //        set { sortCode = value; NotifyPropertyChanged(); }
    //    }

    //    string accountNumber;
    //    public string AccountNumber
    //    {
    //        get { return accountNumber; }
    //        set { accountNumber = value; NotifyPropertyChanged(); }
    //    }

    //    string accountOwner;
    //    public string AccountOwner
    //    {
    //        get { return accountOwner; }
    //        set { accountOwner = value; NotifyPropertyChanged(); }
    //    }

    //    bool paysTaxableInterest;
    //    public bool PaysTaxableInterest
    //    {
    //        get { return paysTaxableInterest; }
    //        set { paysTaxableInterest = value; NotifyPropertyChanged(); }
    //    }

    //    string loginUrl;
    //    public string LoginUrl
    //    {
    //        get { return loginUrl; }
    //        set { loginUrl = value; NotifyPropertyChanged(); }
    //    }

    //    string loginId;
    //    public string LoginId
    //    {
    //        get { return loginId; }
    //        set { loginId = value; NotifyPropertyChanged(); }
    //    }

    //    string passwordHint;
    //    public string PasswordHint
    //    {
    //        get { return passwordHint; }
    //        set { passwordHint = value; NotifyPropertyChanged(); }
    //    }

    //    DateTime openedDate;
    //    public DateTime OpenedDate
    //    {
    //        get { return openedDate; }
    //        set { openedDate = value; NotifyPropertyChanged(); }
    //    }

    //    DateTime? closedDate;
    //    public DateTime? ClosedDate
    //    {
    //        get { return closedDate; }
    //        set { closedDate = value; NotifyPropertyChanged(); }
    //    }

    //    decimal? initialRate;
    //    public decimal? InitialRate
    //    {
    //        get { return initialRate; }
    //        set { initialRate = value; NotifyPropertyChanged(); }
    //    }

    //    DateTime? milestoneDate;
    //    public DateTime? MilestoneDate
    //    {
    //        get { return milestoneDate; }
    //        set { milestoneDate = value; NotifyPropertyChanged(); }
    //    }

    //    string milestoneNotes;
    //    public string MilestoneNotes
    //    {
    //        get { return milestoneNotes; }
    //        set { milestoneNotes = value; NotifyPropertyChanged(); }
    //    }

    //    string notes;
    //    public string Notes
    //    {
    //        get { return notes; }
    //        set { notes = value; NotifyPropertyChanged(); }
    //    }


    //    public ICommand GoOnlineCommand { get; set; }

    //    private void GoOnline()
    //    {
    //        //OpenBrowserEvent ev = new OpenBrowserEvent();
    //        //ev.Url = this.LoginUrl;

    //        //this.eventPublisher.SendMessage<OpenBrowserEvent>(ev);
    //    }



    //}
}
