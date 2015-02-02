using Castle.Core;
using Castle.MicroKernel.Proxy;
using Finances.Core.Interfaces;
using Finances.WinClient.Interceptors;

namespace Finances.WinClient.InterceptorSelectors
{
    public class RepositoryInterceptorSelector : IModelInterceptorsSelector
    {
        public bool HasInterceptors(ComponentModel model)
        {
            int x = 1;
            return typeof(IRepository).IsAssignableFrom(model.Implementation);
        }

        public InterceptorReference[] SelectInterceptors(ComponentModel model, InterceptorReference[] interceptors)
        {
            return new[]
            {
                InterceptorReference.ForType<ErrorHandlingInterceptor>()
            };
        }
    }
}
