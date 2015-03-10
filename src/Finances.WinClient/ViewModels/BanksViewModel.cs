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
    public interface IBanksViewModel //: IListViewModelBase<BankItemViewModel>
    {
        //ActionCommand AddCommand { get; set; }
        //ActionCommand EditCommand { get; set; }
        //ActionCommand DeleteCommand { get; set; }
        //ActionCommand ReloadCommand { get; set; }
        //ObservableCollection<BankItemViewModel> Banks { get; }
        void Open();
        void Close();
    }

    public class BanksViewModel : ListViewModelBase<BankItemViewModel>, IBanksViewModel
    {
        //readonly IBankService bankService;
        readonly IBankRepository bankRepository;
        readonly IMappingEngine mapper;

        readonly IDialogService dialogService;
        readonly IBankEditorViewModelFactory bankEditorViewModelFactory;

        // internal/parallel list of entiities
        Dictionary<int, Bank> banks = new Dictionary<int, Bank>();


        public BanksViewModel(
                        IBankRepository bankRepository,
                        IMappingEngine mapper,
                        IDialogService dialogService,
                        IBankEditorViewModelFactory bankEditorViewModelFactory)
        {
            //this.bankService = bankService;
            this.bankRepository = bankRepository;
            this.mapper = mapper;
            this.dialogService = dialogService;
            this.bankEditorViewModelFactory = bankEditorViewModelFactory;

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
            Open();
        }

        public override void WorkspaceClosed()
        {
            Close();
        }


        public void Open()
        {
            LoadBanks();
        }

        public void Close()
        {
            base.DataList.Clear();
        }


        private void Reload()
        {
            LoadBanks();
        }

        private void LoadBanks()
        {
            //List<BankItemViewModel> banks = null;

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
            var editor = this.bankEditorViewModelFactory.Create();

            // prepare Editor
            editor.InitializeForAddEdit(true);

            // open Editor for adding
            while (this.dialogService.ShowDialogView(editor))
            {
                // map the Editor into an entity
                var newbank = mapper.Map<Bank>(editor);

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

            this.bankEditorViewModelFactory.Release(editor);
        }


        private bool CanEdit()
        {
            return base.GetQtySelected() == 1;
        }
        private void Edit()
        {
            // get item VM being editing
            BankItemViewModel vm = base.GetSelectedItems().First();
            
            // locate the corresponding entity in the internal list
            Bank bank = this.banks[vm.BankId];

            var editor = this.bankEditorViewModelFactory.Create();

            // map the entity into the Editor
            mapper.Map<Bank, BankEditorViewModel>(bank, editor as BankEditorViewModel);

            // prepare the Editor for editing
            editor.InitializeForAddEdit(false);

            // open Editor for editing
            while (this.dialogService.ShowDialogView(editor))
            {
                // map Editor to an entity
                var updbank = mapper.Map<Bank>(editor);

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

            this.bankEditorViewModelFactory.Release(editor);


        }


        private bool CanDelete()
        {
            return base.GetQtySelected() > 0;
        }
        private void Delete()
        {
            string title;
            string message;
            List<BankItemViewModel> sel = base.GetSelectedItems().ToList();

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
