using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.Wpf;
using Finances.WinClient.DomainServices;

namespace Finances.WinClient.ViewModels
{
    public interface IBankTreeViewModel : IListViewModelBase<IBankItemViewModel>
    {
        ObservableCollection<IBankItemViewModel> Banks { get; }
    }

    public class BankTreeViewModel : ListViewModelBase<IBankItemViewModel>, IBankTreeViewModel
    {
        readonly IBankService bankService;
        readonly IBankAccountService bankAccountService;

        public BankTreeViewModel(IBankService bankService,
                    IBankAccountService bankAccountService)
        {
            this.bankService = bankService;
            this.bankAccountService = bankAccountService;

            ReloadCommand = base.AddNewCommand(new ActionCommand(Reload));
        }

        #region Public Properties & Commands

        public ActionCommand ReloadCommand { get; set; }

        public ObservableCollection<IBankItemViewModel> Banks
        {
            get
            {
                return base.DataList;
            }
        }

        #endregion


        public override void WorkspaceOpened()
        {
            LoadBanks();
        }

        public override void WorkspaceClosed()
        {
            ClearBanks();
        }



        private void Reload()
        {
            LoadBanks();
        }

        private void ClearBanks()
        {
            base.DataList.ToList().ForEach(b =>
            {
                b.TreeViewItemExpanded -= TreeViewItemExpandedHandler;
            });

            base.DataList.Clear();
        }

        private void LoadBanks()
        {
            List<IBankItemViewModel> banks = null;

            base.IsBusy = true;

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) =>
                {
                    banks = this.bankService.ReadList();
                };
            bw.RunWorkerCompleted += (s, e) =>
                {
                    ClearBanks();
                    
                    if (banks != null)
                        {
                            banks.ForEach(b =>
                            {
                                b.TreeViewItemExpanded += TreeViewItemExpandedHandler;
                                base.DataList.Add(b);
                            });
                        }

                    base.IsBusy = false;
                };
            
            bw.RunWorkerAsync();


        }

        void TreeViewItemExpandedHandler(object sender, Core.Wpf.Events.TreeViewItemExpandedEventArgs e)
        {
            var x = e.TreeViewItemExpanded;

            var bank = e.TreeViewItemExpanded as IBankItemViewModel;
            if (bank != null)
            {
                if (bank.Children.Count == 0)
                {
                    this.bankAccountService.ReadListByBankId(bank.BankId).ForEach(a =>
                        {
                            bank.Children.Add(a);
                        });
                }
            }

        }



    }
}
