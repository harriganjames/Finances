using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core
{
    public interface IConnection
    {
        string ConnectionString { get; }
    }

    public class Connection : IConnection
    {
        public string ConnectionString
        {
            get
            {
                return "Server=localhost; Database=Finance; Integrated Security=true";// ConfigurationManager.AppSettings["localDB"];
            }
        }
    }
}
