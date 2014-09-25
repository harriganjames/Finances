using System;
using Finances.Core.Entities;
using Finances.WinClient.ViewModels;

namespace Finances.WinClient.Mappers
{
    public interface IBankAccountMapper
    {
        BankAccount Map(IBankAccountEditorViewModel from, BankAccount to);
        IBankAccountItemViewModel Map(BankAccount from, IBankAccountItemViewModel to);
        IBankAccountEditorViewModel Map(BankAccount from, IBankAccountEditorViewModel to);
    }

    public class BankAccountMapper : IBankAccountMapper
    {
        readonly IBankMapper bankMapper;

        public BankAccountMapper(IBankMapper bankMapper)
        {
            this.bankMapper = bankMapper;
        }

        public IBankAccountItemViewModel Map(BankAccount from, IBankAccountItemViewModel to)
        {
            to.BankAccountId = from.BankAccountId;
            to.AccountName = from.Name;
            to.Bank = this.bankMapper.Map(from.Bank, to.Bank);
            to.AccountNumber = from.AccountNumber;
            to.SortCode = from.SortCode;
            to.AccountOwner = from.AccountOwner;
            to.OpenedDate = from.OpenedDate;
            to.ClosedDate = from.ClosedDate;
            to.InitialRate = from.InitialRate;
            to.LoginId = from.LoginID;
            to.LoginUrl = from.LoginURL;
            to.MilestoneDate = from.MilestoneDate;
            to.MilestoneNotes = from.MilestoneNotes;
            to.Notes = from.Notes;
            to.PasswordHint = from.PasswordHint;
            to.PaysTaxableInterest = from.PaysTaxableInterest;
            return to;
        }

        public BankAccount Map(IBankAccountEditorViewModel from, BankAccount to)
        {
            to.BankAccountId = from.BankAccountId;
            to.Name = from.AccountName;
            //to.Bank = this.bankMapper.Map(from.Bank, to.Bank);
            to.Bank.BankId = from.Bank.BankId; // only need BankId for persisting
            to.AccountNumber = from.AccountNumber;
            to.SortCode = from.SortCode;
            to.AccountOwner = from.AccountOwner;
            to.OpenedDate = from.OpenedDate.GetValueOrDefault(DateTime.MinValue);
            to.ClosedDate = from.ClosedDate;
            to.InitialRate = from.InitialRate;
            to.LoginID = from.LoginId;
            to.LoginURL = from.LoginUrl;
            to.MilestoneDate = from.MilestoneDate;
            to.MilestoneNotes = from.MilestoneNotes;
            to.Notes = from.Notes;
            to.PasswordHint = from.PasswordHint;
            to.PaysTaxableInterest = from.PaysTaxableInterest;
            return to;

        }


        public IBankAccountEditorViewModel Map(BankAccount from, IBankAccountEditorViewModel to)
        {
            to.BankAccountId = from.BankAccountId;
            to.AccountName = from.Name;
            to.Bank = this.bankMapper.Map(from.Bank, to.Bank);
            //to.Bank.BankId = from.Bank.BankId; // only need BankId for persisting
            to.AccountNumber = from.AccountNumber;
            to.SortCode = from.SortCode;
            to.AccountOwner = from.AccountOwner;
            to.OpenedDate = from.OpenedDate;
            to.ClosedDate = from.ClosedDate;
            to.InitialRate = from.InitialRate;
            to.LoginId = from.LoginID;
            to.LoginUrl = from.LoginURL;
            to.MilestoneDate = from.MilestoneDate;
            to.MilestoneNotes = from.MilestoneNotes;
            to.Notes = from.Notes;
            to.PasswordHint = from.PasswordHint;
            to.PaysTaxableInterest = from.PaysTaxableInterest;
            return to;
        }
    }
}
