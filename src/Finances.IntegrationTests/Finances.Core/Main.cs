using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.InegrationTests;
using Finances.IntegrationTests.Finances.Core;
using NBehave.Spec;

namespace Finances.IntegrationTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");

            var x = new and_attempting_to_add_and_read_a_value_from_the_database();

            x.MainSetup();

            x.then_the_values_should_be_equal();

            x.MainTeardown();

            Console.WriteLine("End - press a key");
            Console.ReadLine();
        }
    }
}
