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
    public class BankAccountItemViewModel : TreeViewItemViewModelBase
    {
        //readonly IEventPublisher eventPublisher;
        BankAccount entity;

        public BankAccountItemViewModel(BankAccount entity)
        {
            this.entity = entity;

            GoOnlineCommand = base.AddNewCommand(new ActionCommand(GoOnline));
        }


        #region Static

        static BankAccountItemViewModel elsewhere;
        public static BankAccountItemViewModel Elsewhere 
        {
            get
            {
                if(elsewhere==null)
                    elsewhere = new BankAccountItemViewModel(new BankAccount() { BankAccountId = -1, Name = "<elsewhere>" });
                return elsewhere;
            }
        }


        #endregion

        public BankAccount Entity
        {
            get { return entity; }
            set
            {
                entity = value;
                bank = new BankItemViewModel(entity.Bank);
                NotifyAllPropertiesChanged();
            }
        }


        public int BankAccountId
        {
            get { return entity.BankAccountId; }
        }

        public string AccountName
        {
            get { return entity.Name; }
        }

        BankItemViewModel bank;
        public BankItemViewModel Bank
        {
            get 
            {
                if (bank == null)
                    bank = new BankItemViewModel(entity.Bank);
                return bank; 
            }
        }

        public string SortCode
        {
            get { return entity.SortCode; }
        }

        public string AccountNumber
        {
            get { return entity.AccountNumber; }
        }

        public string AccountOwner
        {
            get { return entity.AccountOwner; }
        }

        public bool PaysTaxableInterest
        {
            get { return entity.PaysTaxableInterest; }
        }

        public string LoginUrl
        {
            get { return entity.LoginURL; }
        }

        public string LoginId
        {
            get { return entity.LoginID; }
        }

        public string PasswordHint
        {
            get { return entity.PasswordHint; }
        }

        public DateTime OpenedDate
        {
            get { return entity.OpenedDate; }
        }

        public DateTime? ClosedDate
        {
            get { return entity.ClosedDate; }
        }

        public decimal? InitialRate
        {
            get { return entity.InitialRate; }
        }

        public DateTime? MilestoneDate
        {
            get { return entity.MilestoneDate; }
        }

        public string MilestoneNotes
        {
            get { return entity.MilestoneNotes; }
        }

        public string Notes
        {
            get { return entity.Notes; }
        }


        public ICommand GoOnlineCommand { get; set; }

        private void GoOnline()
        {
            //OpenBrowserEvent ev = new OpenBrowserEvent();
            //ev.Url = this.LoginUrl;

            //this.eventPublisher.SendMessage<OpenBrowserEvent>(ev);
        }

    }
}
