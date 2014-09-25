
using System;
using Castle.DynamicProxy;
using Finances.Core.Wpf;

namespace Finances.WinClient.Interceptors
{
    class ErrorHandlingInterceptor : IInterceptor
    {
        readonly IExceptionService exceptionService;

        public ErrorHandlingInterceptor(IExceptionService exceptionService)
        {
            this.exceptionService = exceptionService;
        }

        public void Intercept(IInvocation invocation)
        {

            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                this.exceptionService.ShowException(e, invocation.TargetType.Name);
                
                object rv = null;

                if(invocation.Method.ReturnType==typeof(bool))
                {
                    rv = false;
                }
                else if (invocation.Method.ReturnType == typeof(int))
                {
                    rv = 0;
                }

                invocation.ReturnValue = rv;
            }



        }
    }
}
