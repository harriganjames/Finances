using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Finances.Persistence.EF.Interfaces;

namespace Finances.Persistence.EF.Interceptors
{
    public class DbEntityValidationExceptionInterceptor : IInterceptor
    {
        readonly IEntityValidationHandler entityValidationHandler;

        public DbEntityValidationExceptionInterceptor(IEntityValidationHandler entityValidationHandler)
        {
            this.entityValidationHandler = entityValidationHandler;
        }


        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (DbEntityValidationException e)
            {
                //this.entityValidationHandler.HandleDbEntityValidationException(e);
                throw new Exception(this.entityValidationHandler.ComposeErrorMessage(e));
            }

        }
    }
}
