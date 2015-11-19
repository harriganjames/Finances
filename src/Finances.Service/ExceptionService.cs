using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Finances.Interface;

namespace Finances.Service
{
    //public interface IExceptionService
    //{
    //    void ShowException(Exception e);
    //    void ShowException(Exception e, string title);
    //}

    public class ExceptionService : IExceptionService
    {
        readonly IDialogService dialogService;

        public ExceptionService(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }


        public void ShowException(Exception e, string title)
        {
            string msg = GetErrors(e);

            dialogService.ShowMessageBox(title, msg, MessageBoxButtonEnum.OK);

        }
        public void ShowException(Exception e)
        {
            this.ShowException(e, "Error");
        }


        private string GetErrors(Exception e)
        {
            string errs;

            errs = e.Message;
            if (e.InnerException != null)
                errs += "\r\r" + GetErrors(e.InnerException);

            return errs;
        }

    }
}
