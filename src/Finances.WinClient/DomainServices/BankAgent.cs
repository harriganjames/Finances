using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Core.Wpf;
using Finances.WinClient.Factories;

namespace Finances.WinClient.DomainServices
{
    public interface IBankAgent
    {
        /// <summary>
        /// Open dialog to add a new Bank
        /// Persists the Bank to the repository
        /// </summary>
        /// <returns>new BankId or 0 if aborted/failed</returns>
        int Add();

        /// <summary>
        /// Opens dialog to edit the Bank
        /// Persists the Bank to the repository
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns>true if edit was successful</returns>
        bool Edit(int id);

        bool Delete(List<int> ids);

    }

    public class BankAgent : IBankAgent
    {
        readonly IBankRepository bankRepository;
        readonly IDialogService dialogService;
        readonly IBankEditorViewModelFactory bankEditorViewModelFactory;

        public BankAgent(
                        IBankRepository bankRepository,
                        IDialogService dialogService,
                        IBankEditorViewModelFactory bankEditorViewModelFactory
                        )
        {
            this.bankRepository = bankRepository;
            this.dialogService = dialogService;
            this.bankEditorViewModelFactory = bankEditorViewModelFactory;
        }



        public int Add()
        {
            int id = 0;

            var entity = new Bank();

            var editor = this.bankEditorViewModelFactory.Create(entity);

            editor.InitializeForAddEdit(true);

            while (this.dialogService.ShowDialogView(editor))
            {
                id = this.bankRepository.Add(entity);

                if (id>0)
                {
                    break;
                }
            }

            this.bankEditorViewModelFactory.Release(editor);

            return id;
        }





        public bool Edit(int bankId)
        {
            bool result = false;

            Bank entity = this.bankRepository.Read(bankId);

            var editor = this.bankEditorViewModelFactory.Create(entity);

            editor.InitializeForAddEdit(false);

            while (this.dialogService.ShowDialogView(editor))
            {
                result = this.bankRepository.Update(entity);
                if (result)
                {
                    break;
                }
            }

            this.bankEditorViewModelFactory.Release(editor);

            return result;
        }



        public bool Delete(List<int> ids)
        {
            bool result = false;
            string title;
            string message;

            if (ids.Count() == 0)
            {
                throw new Exception("Delete Bank: nothing to delete");
            }

            if (ids.Count() == 1)
            {
                title = "Delete Bank";
                Bank entity = this.bankRepository.Read(ids[0]);
                message = String.Format("Please confirm deletion of bank: {0}", entity.Name);
            }
            else
            {
                title = "Delete Banks";
                message = String.Format("Please confirm deletion of {0} banks?", ids.Count());
            }


            if (this.dialogService.ShowMessageBox(title, message, MessageBoxButtonEnum.YesNo) == MessageBoxResultEnum.Yes)
            {
                result = this.bankRepository.Delete(ids);
            }

            return result;
        }





    }
}
