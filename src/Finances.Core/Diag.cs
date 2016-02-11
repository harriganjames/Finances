using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Finances.Core
{
    public static class Diag
    {
        public static void ThreadPrint(string format, params object[] pars)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("{0} ", DateTime.Now.ToString("hh:mm:ss.ms"));

            sb.AppendFormat("{0} ({1}) ", Thread.CurrentThread.IsBackground ? "Background" : "Main", Thread.CurrentThread.ManagedThreadId);

            sb.AppendFormat(format, pars);

            Debug.WriteLine(sb.ToString());

        }

    }
}
