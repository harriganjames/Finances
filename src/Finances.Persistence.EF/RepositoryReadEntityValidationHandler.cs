using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.Entities;
using Finances.Core.Interfaces;
using Finances.Persistence.EF.Interfaces;

// Inject:
// RepositoryReadEntityValidationHandler( CashflowReporsitory(factory,mapper), EntityValidationHandler )


namespace Finances.Persistence.EF
{
    public class RepositoryReadEntityValidationHandler<TEntity> : IRepositoryRead<TEntity> where TEntity : Entity
    {
        readonly IRepositoryRead<TEntity> decorated;
        readonly IEntityValidationHandler entityValidationHandler;

        public RepositoryReadEntityValidationHandler(
                        IRepositoryRead<TEntity> decorated,
                        IEntityValidationHandler entityValidationHandler)
        {
            this.decorated = decorated;
            this.entityValidationHandler = entityValidationHandler;
        }


        TEntity IRepositoryRead<TEntity>.Read(int dataId)
        {
            TEntity entity = default(TEntity);
            try
            {
                entity = decorated.Read(dataId);
            }
            catch (DbEntityValidationException e)
            {
                this.entityValidationHandler.HandleDbEntityValidationException(e);
            }
            return entity;
        }

        List<TEntity> IRepositoryRead<TEntity>.ReadList()
        {
            List<TEntity> list = null;
            try
            {
                list = decorated.ReadList();
            }
            catch (DbEntityValidationException e)
            {
                this.entityValidationHandler.HandleDbEntityValidationException(e);
            }
            return list;
        }

        List<Core.Entities.DataIdName> IRepositoryRead<TEntity>.ReadListDataIdName()
        {
            List<Core.Entities.DataIdName> list = null;
            try
            {
                list = decorated.ReadListDataIdName();
            }
            catch (DbEntityValidationException e)
            {
                this.entityValidationHandler.HandleDbEntityValidationException(e);
            }
            return list;
        }
    }
}
