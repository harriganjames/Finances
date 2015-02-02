using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Automation;
using System.Windows.Automation.Text;
using System.Diagnostics;
using System.Threading;
                                     
namespace Finances.UiTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var aeDesktop = AutomationElement.RootElement;

            var p = Process.Start(@"C:\Share\Projects\DevProjects\Finances\src\Finances.WinClient\bin\Debug\Finances.WinClient.exe");

            for (int i = 0; i < 100; i++)
            {
                if (p.MainWindowHandle != null)
                    break;
                Thread.Sleep(100);
            }

            var aeProcess = AutomationElement.FromHandle(p.MainWindowHandle);


           

        }
    }
}
