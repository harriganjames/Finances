
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
    public interface IBankAccountAgent
    {
        /// <summary>
        /// Open dialog to add a new BankAccount
        /// Persists the BankAccount to the repository
        /// </summary>
        /// <returns>new BankAccountId or 0 if aborted/failed</returns>
        int Add();

        /// <summary>
        /// Opens dialog to edit the BankAccount
        /// Persists the BankAccount to the repository
        /// </summary>
        /// <param name="bankAccountId"></param>
        /// <returns>true if edit was successful</returns>
        bool Edit(int id);

        bool Delete(List<int> ids);

    }

    public class BankAccountAgent : IBankAccountAgent
    {
        readonly IBankAccountRepository bankAccountRepository;
        readonly IDialogService dialogService;
        readonly IBankAccountEditorViewModelFactory bankAccountEditorViewModelFactory;

        public BankAccountAgent(
                        IBankAccountRepository bankAccountRepository,
                        IDialogService dialogService,
                        IBankAccountEditorViewModelFactory bankEditorViewModelFactory
                        )
        {
            this.bankAccountRepository = bankAccountRepository;
            this.dialogService = dialogService;
            this.bankAccountEditorViewModelFactory = bankEditorViewModelFactory;
        }



        public int Add()
        {
            int id = 0;

            var entity = new BankAccount();

            var editor = this.bankAccountEditorViewModelFactory.Create(entity);

            editor.InitializeForAddEdit(true);

            while (this.dialogService.ShowDialogView(editor))
            {
                id = this.bankAccountRepository.Add(entity);

                if (id>0)
                {
                    break;
                }
            }

            this.bankAccountEditorViewModelFactory.Release(editor);

            return id;
        }





        public bool Edit(int id)
        {
            bool result = false;

            BankAccount entity = this.bankAccountRepository.Read(id);

            var editor = this.bankAccountEditorViewModelFactory.Create(entity);

            editor.InitializeForAddEdit(false);

            while (this.dialogService.ShowDialogView(editor))
            {
                result = this.bankAccountRepository.Update(entity);
                if (result)
                {
                    break;
                }
            }

            this.bankAccountEditorViewModelFactory.Release(editor);

            return result;
        }



        public bool Delete(List<int> ids)
        {
            bool result = false;
            string title;
            string message;

            if (ids.Count() == 0)
            {
                throw new Exception("Delete BankAccount: nothing to delete");
            }

            if (ids.Count() == 1)
            {
                title = "Delete BankAccount";
                BankAccount entity = this.bankAccountRepository.Read(ids[0]);
                message = String.Format("Please confirm deletion of bank account: {0}", entity.Name);
            }
            else
            {
                title = "Delete BankAccounts";
                message = String.Format("Please confirm deletion of {0} bank accounts?", ids.Count());
            }


            if (this.dialogService.ShowMessageBox(title, message, MessageBoxButtonEnum.YesNo) == MessageBoxResultEnum.Yes)
            {
                result = this.bankAccountRepository.Delete(ids);
            }

            return result;
        }





    }
}
