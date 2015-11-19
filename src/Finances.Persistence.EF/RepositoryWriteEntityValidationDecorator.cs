using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Persistence.EF.Interfaces;

namespace Finances.Persistence.EF
{
    public class RepositoryWriteEntityValidationHandler<TEntity> : IRepositoryWrite<TEntity>, IRepository where TEntity : Entity
    {
        readonly IRepositoryWrite<TEntity> decorated;
        readonly IEntityValidationHandler entityValidationHandler;

        public RepositoryWriteEntityValidationHandler(
                        IRepositoryWrite<TEntity> decorated,
                        IEntityValidationHandler entityValidationHandler)
        {
            this.decorated = decorated;
            this.entityValidationHandler = entityValidationHandler;
        }


        public int Add(TEntity data)
        {
            int rv = default(int);
            try
            {
                rv = decorated.Add(data);
            }
            catch (DbEntityValidationException e)
            {
                this.entityValidationHandler.HandleDbEntityValidationException(e);
            }
            return rv;
        }

        public bool Update(TEntity data)
        {
            bool rv = default(bool);
            try
            {
                rv = decorated.Update(data);
            }
            catch (DbEntityValidationException e)
            {
                this.entityValidationHandler.HandleDbEntityValidationException(e);
            }
            return rv;
        }

        public bool Delete(TEntity data)
        {
            bool rv = default(bool);
            try
            {
                rv = decorated.Delete(data);
            }
            catch (DbEntityValidationException e)
            {
                this.entityValidationHandler.HandleDbEntityValidationException(e);
            }
            return rv;
        }

        public bool Delete(List<int> ids)
        {
            bool rv = default(bool);
            try
            {
                rv = decorated.Delete(ids);
            }
            catch (DbEntityValidationException e)
            {
                this.entityValidationHandler.HandleDbEntityValidationException(e);
            }
            return rv;
        }
    }
}
