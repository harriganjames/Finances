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

namespace Finances.WinClient.ViewModels
{
    public interface IBanksViewModelNoService : IListViewModelBase<IBankItemViewModel>
    {
        ActionCommand AddCommand { get; set; }
        ActionCommand EditCommand { get; set; }
        ActionCommand DeleteCommand { get; set; }
        ActionCommand ReloadCommand { get; set; }
        ObservableCollection<IBankItemViewModel> Banks { get; }
    }

    public class BanksViewModelNoService : ListViewModelBase<IBankItemViewModel>, IBanksViewModelNoService
    {
        //readonly IBankService bankService;
        readonly IBankRepository bankRepository;
        readonly IMappingEngine mapper;

        readonly IDialogService dialogService;
        readonly IBankEditorViewModel bankEditorViewModel;

        // internal/parallel list of entiities
        Dictionary<int, Bank> banks = new Dictionary<int, Bank>();


        public BanksViewModelNoService(
                        IBankRepository bankRepository,
                        IMappingEngine mapper,
                        IDialogService dialogService,
                        IBankEditorViewModel bankEditorViewModel)
        {
            //this.bankService = bankService;
            this.bankRepository = bankRepository;
            this.mapper = mapper;
            this.dialogService = dialogService;
            this.bankEditorViewModel = bankEditorViewModel;

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
            base.DataList.Clear();
        }

        private void Reload()
        {
            LoadBanks();
        }

        private void LoadBanks()
        {
            //List<IBankItemViewModel> banks = null;

            base.IsBusy = true;

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) =>
                {
                    //banks = this.bankService.ReadList();
                    //System.Threading.Thread.Sleep(5000);

                    banks.Clear();

                    var ebanks = this.bankRepository.ReadList();

                    if (ebanks != null)
                        ebanks.ForEach(eb => banks.Add(eb.BankId, eb));

                    //mapper.Map<List<BankItemViewModel>>(ebanks);

                };
            bw.RunWorkerCompleted += (s, e) =>
                {
                    base.DataList.Clear();
                    if (banks != null)
                    {
                        banks.Values.ToList().ForEach(b =>
                        {
                            base.DataList.Add(mapper.Map<BankItemViewModel>(b));
                        });
                    }

                    base.IsBusy = false;
                };
            
            bw.RunWorkerAsync();


            //await Task.Run(() =>
            //{
            //    banks = this.bankService.ReadList();
            //    System.Threading.Thread.Sleep(5000);
            //    int i = 0;

            //    base.DataList.Clear();
            //    if (banks != null)
            //    {
            //        banks.ForEach(b =>
            //        {
            //            base.DataList.Add(b);
            //        });
            //    }

            //});
                

            //base.IsBusy = false;

        }



        private void Add()
        {
            // prepare Editor
            this.bankEditorViewModel.InitializeForAddEdit(true);

            // open Editor for adding
            while (this.dialogService.ShowDialogView(this.bankEditorViewModel))
            {
                // map the Editor into an entity
                var newbank = mapper.Map<Bank>(this.bankEditorViewModel);

                // save the entity and get result
                bool result = this.bankRepository.Add(newbank) > 0;

                if (result)
                {
                    // add entity to internal list
                    this.banks.Add(newbank.BankId, newbank);

                    // map new entity to a VM
                    var newvm = mapper.Map<BankItemViewModel>(newbank);

                    // add VM to public list
                    base.DataList.Add(newvm);
                    // reset flags
                    base.DataList.ToList().ForEach(i => i.IsSelected = false);
                    // select the new VM
                    newvm.IsSelected = true;
                    break;
                }
            }

        }


        private bool CanEdit()
        {
            return base.GetQtySelected() == 1;
        }
        private void Edit()
        {
            // get item VM being editing
            IBankItemViewModel vm = base.GetSelectedItems().First();
            
            // locate the corresponding entity in the internal list
            Bank bank = this.banks[vm.BankId];

            // map the entity into the Editor
            mapper.Map<Bank, BankEditorViewModel>(bank, this.bankEditorViewModel as BankEditorViewModel);

            // prepare the Editor for editing
            this.bankEditorViewModel.InitializeForAddEdit(false);

            // open Editor for editing
            while (this.dialogService.ShowDialogView(this.bankEditorViewModel))
            {
                // map Editor to an entity
                var updbank = mapper.Map<Bank>(this.bankEditorViewModel);

                // save the entity and get the result
                bool result = this.bankRepository.Update(updbank);
                if (result)
                {
                    // re-read the entity into the internal list (may have changed values set in the database)
                    this.banks[vm.BankId] = this.bankRepository.Read(vm.BankId);
                    // map the entity back to the VM
                    mapper.Map<Bank, BankItemViewModel>(this.banks[vm.BankId], vm as BankItemViewModel);
                    break;
                }
            }


        }


        private bool CanDelete()
        {
            return base.GetQtySelected() > 0;
        }
        private void Delete()
        {
            string title;
            string message;
            List<IBankItemViewModel> sel = base.GetSelectedItems().ToList();

            if (sel.Count() == 0)
            {
                throw new Exception("Delete Bank: nothing selected");
            }

            if (sel.Count() == 1)
            {
                title = "Delete Bank";
                message = String.Format("Please confirm deletion of bank: {0}", sel.First().Name);
            }
            else
            {
                title = "Delete Banks";
                message = String.Format("Please confirm deletion of {0} banks?", sel.Count());
            }


            if (this.dialogService.ShowMessageBox(title, message, MessageBoxButtonEnum.YesNo) == MessageBoxResultEnum.Yes)
            {
                base.IsBusy = true;

                foreach (var vm in sel)
                {
                    // map the VM to an entity
                    Bank bank = mapper.Map<Bank>(vm);
                    // delete from database
                    if (this.bankRepository.Delete(bank))
                    {
                        // remove from public list
                        base.DataList.Remove(vm);
                        // remove from internal list
                        this.banks.Remove(bank.BankId);
                    }
                    else
                        break;
                }

                base.IsBusy = false;
            }
        }


        //public override string ToString()
        //{
        //        return "Hello";
        //}

    }
}
