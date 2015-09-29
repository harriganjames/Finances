using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core;

namespace Finances.IntegrationTests.MS
{
    public class IntegrationConnection : IConnection
    {
        public string ConnectionString
        {
            get
            {
                string connectionString = "data source=MIKE_LAPTOP;initial catalog=FinanceINT;integrated security=True;App=MSTest;";
                return connectionString;
            }
        }
    }
}
