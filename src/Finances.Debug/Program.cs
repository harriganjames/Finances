using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Finances.Utility;
using System.Threading.Tasks.Dataflow;

namespace Finances.Debug
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");

            DoDebounce();

            Console.WriteLine("End - press a key");
            Console.ReadLine();

        }

        static ActionBlock<string> ab = new ActionBlock<string>(new Action<string>(Refresh));

        static void DoDebounce()
        {
            var de = new DebounceExecutor<string>();


            de.Initialize(new TimeSpan(0, 0, 2), Execute);

            de.Execute("o");
            de.Execute("on");
            de.Execute("one");

            Task.Delay(1000 * 3).Wait();

            de.Execute("t");
            de.Execute("tw");
            de.Execute("two");



        }


        static void Execute(string s)
        {
            Console.WriteLine("Post {0} - start", s);
            ab.Post(s);
            Console.WriteLine("Post {0} - end", s);
        }


        static void Refresh(string s)
        {
            Console.WriteLine("Refresh {0} - start", s);
            Task.Delay(1000 * 5).Wait();
            Console.WriteLine("Refresh {0} - end", s);
        }

    }
}
