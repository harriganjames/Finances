using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Core.Wpf;
using Finances.WinClient.DomainServices;
using Finances.WinClient.Factories;

namespace Finances.WinClient.ViewModels
{

    public class DashboardViewModel : Workspace
    {
        readonly IBanksViewModel banksViewModel;
        readonly IBankAccountsViewModel bankAccountsViewModel;
        readonly ITransferListViewModel transferListViewModel;

        public DashboardViewModel(
                    IBanksViewModel banksViewModel,
                    IBankAccountsViewModel bankAccountsViewModel,
                    ITransferListViewModel transferListViewModel
                    )
        {
            this.banksViewModel = banksViewModel;
            this.bankAccountsViewModel = bankAccountsViewModel;
            this.transferListViewModel = transferListViewModel;

            ReloadCommand = base.AddNewCommand(new ActionCommand(Reload));
            AddCommand = base.AddNewCommand(new ActionCommand(Add));
            EditCommand = base.AddNewCommand(new ActionCommand(Edit,CanEdit));
            DeleteCommand = base.AddNewCommand(new ActionCommand(Delete, CanDelete));
        }


        #region Commmands

        public ActionCommand ReloadCommand { get; set; }
        public ActionCommand AddCommand { get; set; }
        public ActionCommand EditCommand { get; set; }
        public ActionCommand DeleteCommand { get; set; }

        #endregion

        #region PublicProperties



        public IBanksViewModel BanksViewModel
        {
          get 
          { 
              return banksViewModel; 
          }
        }

        public IBankAccountsViewModel BankAccountsViewModel
        {
            get
            {
                return bankAccountsViewModel;
            }
        }

        public ITransferListViewModel TransferListViewModel
        {
            get
            {
                return transferListViewModel;
            }
        }

        #endregion


        public override void WorkspaceOpened()
        {
            this.banksViewModel.Open();
            this.bankAccountsViewModel.Open();
            this.transferListViewModel.Open();
        }

        public override void WorkspaceClosed()
        {
            this.banksViewModel.Close();
            this.bankAccountsViewModel.Close();
            this.transferListViewModel.Close();
        }





        private void Reload()
        {
        }



        private void Add()
        {
        }


        private bool CanEdit()
        {
            return true;
        }
        private void Edit()
        {

        }


        private bool CanDelete()
        {
            return true;
        }
        private void Delete()
        {
        }


    }
}
