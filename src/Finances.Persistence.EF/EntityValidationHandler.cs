using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Finances.Persistence.EF.Interfaces;

namespace Finances.Persistence.EF
{
    public class EntityValidationHandler : IEntityValidationHandler
    {
        public void HandleDbEntityValidationException(DbEntityValidationException e)
        {
            var message = new StringBuilder();

            foreach (var err in e.EntityValidationErrors)
            {
                foreach (var inner in err.ValidationErrors)
                {
                    message.AppendLine(inner.ErrorMessage);
                }
            }
            throw new Exception(message.ToString());
        }
    }
}
