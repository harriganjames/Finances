
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Core.Wpf;
using Finances.Interface;
using Finances.WinClient.Factories;

namespace Finances.WinClient.DomainServices
{
    public interface IBalanceDateAgent
    {
        /// <summary>
        /// Open dialog to add a new BalanceDate
        /// Persists the BalanceDate to the repository
        /// </summary>
        /// <returns>new BalanceDateId or 0 if aborted/failed</returns>
        int Add();

        /// <summary>
        /// Opens dialog to edit the BalanceDate
        /// Persists the BalanceDate to the repository
        /// </summary>
        /// <param name="balanceDateId"></param>
        /// <returns>true if edit was successful</returns>
        bool Edit(int id);

        bool Delete(List<int> ids);

    }

    public class BalanceDateAgent : IBalanceDateAgent
    {
        readonly IBalanceDateRepository balanceDateRepository;
        readonly IDialogService dialogService;
        readonly IBalanceDateEditorViewModelFactory balanceDateEditorViewModelFactory;

        public BalanceDateAgent(
                        IBalanceDateRepository balanceDateRepository,
                        IDialogService dialogService,
                        IBalanceDateEditorViewModelFactory bankEditorViewModelFactory
                        )
        {
            this.balanceDateRepository = balanceDateRepository;
            this.dialogService = dialogService;
            this.balanceDateEditorViewModelFactory = bankEditorViewModelFactory;
        }



        public int Add()
        {
            int id = 0;

            var entity = new BalanceDate();

            var editor = this.balanceDateEditorViewModelFactory.Create(entity);

            editor.InitializeForAddEdit(true);

            while (this.dialogService.ShowDialogView(editor))
            {
                id = this.balanceDateRepository.Add(entity);

                if (id > 0)
                {
                    break;
                }
            }

            this.balanceDateEditorViewModelFactory.Release(editor);

            return id;
        }





        public bool Edit(int id)
        {
            bool result = false;

            BalanceDate entity = this.balanceDateRepository.Read(id);

            var editor = this.balanceDateEditorViewModelFactory.Create(entity);

            editor.InitializeForAddEdit(false);

            while (this.dialogService.ShowDialogView(editor))
            {
                result = this.balanceDateRepository.Update(entity);
                if (result)
                {
                    break;
                }
            }

            this.balanceDateEditorViewModelFactory.Release(editor);

            return result;
        }



        public bool Delete(List<int> ids)
        {
            bool result = false;
            string title;
            string message;

            if (ids.Count() == 0)
            {
                throw new Exception("Delete BalanceDate: nothing to delete");
            }

            if (ids.Count() == 1)
            {
                title = "Delete BalanceDate";
                BalanceDate entity = this.balanceDateRepository.Read(ids[0]);
                message = String.Format("Please confirm deletion of Balance Date: {0:yyyy-MM-dd}", entity.DateOfBalance);
            }
            else
            {
                title = "Delete BalanceDates";
                message = String.Format("Please confirm deletion of {0} bank accounts?", ids.Count());
            }


            if (this.dialogService.ShowMessageBox(title, message, MessageBoxButtonEnum.YesNo) == MessageBoxResultEnum.Yes)
            {
                result = this.balanceDateRepository.Delete(ids);
            }

            return result;
        }


    }
}
