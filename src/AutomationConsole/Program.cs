using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace AutomationConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TestFinances();
        }




        static void TestFinances()
        {
            var aeDesktop = AutomationElement.RootElement;

            var p = Process.Start(@"C:\Share\Projects\DevProjects\Finances\src\Finances.WinClient\bin\Debug\Finances.WinClient.exe");

            for (int i = 0; i < 100; i++)
            {
                if (p.MainWindowHandle.ToInt32()>0)
                    break;
                Thread.Sleep(100);
            }


            Console.WriteLine("Handle={0}", p.MainWindowHandle.ToString());

            var aeProcess = AutomationElement.FromHandle(p.MainWindowHandle);



            var aeAll = aeProcess.FindAll(TreeScope.Descendants, Condition.TrueCondition);

            foreach (var item in aeAll)
            {
                var ae = item as AutomationElement;
                Console.WriteLine("ClassName={0} AutomationId={1} Name={2}", 
                    ae.Current.ClassName, 
                    ae.Current.AutomationId,
                    ae.GetCurrentPropertyValue(AutomationElement.NameProperty)
                    );

            }



            var aeLoginButtons = aeProcess.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button));

            //var aeLoginButton = aeProcess.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Login"));

            
            var aeLoginButton = aeProcess.FindFirst(TreeScope.Descendants
                    , new AndCondition(new Condition[] 
                        { 
                            new PropertyCondition(AutomationElement.NameProperty,"Login"), 
                            new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button)
                        })
            );



            var ipLoginButton = (InvokePattern)aeLoginButton.GetCurrentPattern(InvokePattern.Pattern);


            ipLoginButton.Invoke();


            Console.ReadLine();

            p.CloseMainWindow();
        }

        
    }
}
