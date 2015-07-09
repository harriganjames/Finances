using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Persistence.EF
{
    public interface IModelContextFactory
    {
        FinanceEntities CreateContext();
    }

    public class ModelContextFactory : IModelContextFactory
    {
        private readonly string EFConnectionString;

        public ModelContextFactory(string connectionString)
        {
            this.EFConnectionString = String.Format("metadata=res://*/Model.csdl|res://*/Model.ssdl|res://*/Model.msl;provider=System.Data.SqlClient;provider connection string=\"{0}\";",connectionString);
        }

        public FinanceEntities CreateContext()
        {
            var context = new FinanceEntities(this.EFConnectionString);
            context.Configuration.LazyLoadingEnabled = false;
            return context;
        }
    }
}
