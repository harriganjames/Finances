using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Finances.Core.Interfaces;
using Finances.Core.Wpf;
using Finances.WinClient.DomainServices;

namespace Finances.WinClient.ViewModels
{
    //public interface IBankTreeViewModel : IListViewModelBase<BankItemViewModel>
    //{
    //    ObservableCollection<BankItemViewModel> Banks { get; }
    //}

    public class BankTreeViewModel : ListViewModelBase<BankItemViewModel>//, IBankTreeViewModel
    {

        readonly IBankRepository bankRepository;
        readonly IBankAccountRepository bankAccountRepository;
        readonly IMappingEngine mapper;

        public BankTreeViewModel(IBankRepository bankRepository,
                    IBankAccountRepository bankAccountRepository,
                    IMappingEngine mapper)
        {
            this.bankRepository = bankRepository;
            this.bankAccountRepository = bankAccountRepository;
            this.mapper = mapper;

            ReloadCommand = base.AddNewCommand(new ActionCommand(Reload));
        }

        #region Public Properties & Commands

        public ActionCommand ReloadCommand { get; set; }

        public ObservableCollection<BankItemViewModel> Banks
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
            List<BankItemViewModel> banks = null;

            base.IsBusy = true;

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) =>
                {
                    var entities = this.bankRepository.ReadList();
                    banks = mapper.Map<List<BankItemViewModel>>(entities);
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

            var bank = e.TreeViewItemExpanded as BankItemViewModel;
            if (bank != null)
            {
                if (bank.Children.Count == 0)
                {
                    var entities = this.bankAccountRepository.ReadListByBankId(bank.BankId);

                    entities.ForEach(ent=> bank.Children.Add(this.mapper.Map<BankAccountItemViewModel>(ent)));

                    //this.bankAccountService.ReadListByBankId(bank.BankId).ForEach(a =>
                    //    {
                    //        bank.Children.Add(a);
                    //    });
                }
            }

        }



    }
}
