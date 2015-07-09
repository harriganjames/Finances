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
    public interface ITransferAgent
    {
        /// <summary>
        /// Open dialog to add a new Transfer
        /// Persists the Transfer to the repository
        /// </summary>
        /// <returns>new TransferId or 0 if aborted/failed</returns>
        int Add();

        /// <summary>
        /// Opens dialog to edit the Transfer
        /// Persists the Transfer to the repository
        /// </summary>
        /// <param name="transferId"></param>
        /// <returns>true if edit was successful</returns>
        bool Edit(int id);

        bool Delete(List<int> ids);

    }

    public class TransferAgent : ITransferAgent
    {
        readonly ITransferRepository transferRepository;
        readonly IDialogService dialogService;
        readonly ITransferEditorViewModelFactory transferEditorViewModelFactory;

        public TransferAgent(
                        ITransferRepository transferRepository,
                        IDialogService dialogService,
                        ITransferEditorViewModelFactory bankEditorViewModelFactory
                        )
        {
            this.transferRepository = transferRepository;
            this.dialogService = dialogService;
            this.transferEditorViewModelFactory = bankEditorViewModelFactory;
        }



        public int Add()
        {
            int id = 0;

            var entity = new Transfer();

            var editor = this.transferEditorViewModelFactory.Create(entity);

            editor.InitializeForAddEdit(true);

            while (this.dialogService.ShowDialogView(editor))
            {
                id = this.transferRepository.Add(entity);

                if (id>0)
                {
                    break;
                }
            }

            this.transferEditorViewModelFactory.Release(editor);

            return id;
        }





        public bool Edit(int id)
        {
            bool result = false;

            Transfer entity = this.transferRepository.Read(id);

            var editor = this.transferEditorViewModelFactory.Create(entity);

            editor.InitializeForAddEdit(false);

            while (this.dialogService.ShowDialogView(editor))
            {
                result = this.transferRepository.Update(entity);
                if (result)
                {
                    break;
                }
            }

            this.transferEditorViewModelFactory.Release(editor);

            return result;
        }



        public bool Delete(List<int> ids)
        {
            bool result = false;
            string title;
            string message;

            if (ids.Count() == 0)
            {
                throw new Exception("Delete Transfer: nothing to delete");
            }

            if (ids.Count() == 1)
            {
                title = "Delete Transfer";
                Transfer entity = this.transferRepository.Read(ids[0]);
                message = String.Format("Please confirm deletion of transfer: {0}", entity.Name);
            }
            else
            {
                title = "Delete Transfers";
                message = String.Format("Please confirm deletion of {0} transfers?", ids.Count());
            }


            if (this.dialogService.ShowMessageBox(title, message, MessageBoxButtonEnum.YesNo) == MessageBoxResultEnum.Yes)
            {
                result = this.transferRepository.Delete(ids);
            }

            return result;
        }





    }
}
