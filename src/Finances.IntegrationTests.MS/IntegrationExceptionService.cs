using Finances.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Finances.IntegrationTests.MS
{
    public class IntegrationExceptionService : IExceptionService
    {
        public void ShowException(Exception e)
        {
            ShowException(e, "Exception");
        }

        public void ShowException(Exception e, string title)
        {
            Assert.Fail(String.Format("{0}: {1}", title, e.Message));
        }
    }
}
