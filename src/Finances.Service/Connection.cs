using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Interface;

namespace Finances.Service
{
    //public interface IConnection
    //{
    //    string ConnectionString { get; }
    //}

    public class Connection : IConnection
    {
        public string ConnectionString
        {
            get
            {
                string connection;
                connection = Environment.GetEnvironmentVariable("FINANCES_CONNECTION");
                if (connection == null)
                {
                    connection = ConfigurationManager.ConnectionStrings["Finance"].ConnectionString;
                }
                return connection;
                //return "Server=localhost; Database=Finance; Integrated Security=true";// ConfigurationManager.AppSettings["localDB"];
            }
        }
    }
}
