﻿using System;
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
    public interface ICashflowAgent
    {
        /// <summary>
        /// Open dialog to add a new Cashflow
        /// Persists the Cashflow to the repository
        /// </summary>
        /// <returns>new CashflowId or 0 if aborted/failed</returns>
        int Add();

        /// <summary>
        /// Opens dialog to edit the Cashflow
        /// Persists the Cashflow to the repository
        /// </summary>
        /// <param name="cashflowId"></param>
        /// <returns>true if edit was successful</returns>
        bool Edit(int id);

        bool Delete(List<int> ids);

    }

    public class CashflowAgent : ICashflowAgent
    {
        readonly ICashflowRepository cashflowRepository;
        readonly IDialogService dialogService;
        readonly ICashflowEditorViewModelFactory cashflowEditorViewModelFactory;

        public CashflowAgent(
                        ICashflowRepository cashflowRepository,
                        IDialogService dialogService,
                        ICashflowEditorViewModelFactory bankEditorViewModelFactory
                        )
        {
            this.cashflowRepository = cashflowRepository;
            this.dialogService = dialogService;
            this.cashflowEditorViewModelFactory = bankEditorViewModelFactory;
        }



        public int Add()
        {
            int id = 0;

            var entity = new Cashflow();

            var editor = this.cashflowEditorViewModelFactory.Create(entity);

            editor.InitializeForAddEdit(true);

            while (this.dialogService.ShowDialogView(editor))
            {
                id = this.cashflowRepository.Add(entity);

                if (id>0)
                {
                    break;
                }
            }

            this.cashflowEditorViewModelFactory.Release(editor);

            return id;
        }





        public bool Edit(int id)
        {
            bool result = false;

            Cashflow entity = this.cashflowRepository.Read(id);

            var editor = this.cashflowEditorViewModelFactory.Create(entity);

            editor.InitializeForAddEdit(false);

            while (this.dialogService.ShowDialogView(editor))
            {
                result = this.cashflowRepository.Update(entity);
                if (result)
                {
                    break;
                }
            }

            this.cashflowEditorViewModelFactory.Release(editor);

            return result;
        }



        public bool Delete(List<int> ids)
        {
            bool result = false;
            string title;
            string message;

            if (ids.Count() == 0)
            {
                throw new Exception("Delete Cashflow: nothing to delete");
            }

            if (ids.Count() == 1)
            {
                title = "Delete Cashflow";
                Cashflow entity = this.cashflowRepository.Read(ids[0]);
                message = String.Format("Please confirm deletion of cashflow: {0}", entity.Name);
            }
            else
            {
                title = "Delete Cashflows";
                message = String.Format("Please confirm deletion of {0} cashflows?", ids.Count());
            }


            if (this.dialogService.ShowMessageBox(title, message, MessageBoxButtonEnum.YesNo) == MessageBoxResultEnum.Yes)
            {
                result = this.cashflowRepository.Delete(ids);
            }

            return result;
        }





    }
}