//using Castle.Core;
//using Castle.MicroKernel.Proxy;
//using Finances.Core.Interfaces;
//using Finances.Interface;
//using Finances.WinClient.Interceptors;

//namespace Finances.WinClient.InterceptorSelectors
//{
//    public class ErrorHandlingInterceptorSelector : IModelInterceptorsSelector
//    {
//        public bool HasInterceptors(ComponentModel model)
//        {
//            return typeof(IExceptionHandler).IsAssignableFrom(model.Implementation);
//        }

//        public InterceptorReference[] SelectInterceptors(ComponentModel model, InterceptorReference[] interceptors)
//        {
//            return new[]
//            {
//                InterceptorReference.ForType<ErrorHandlingInterceptor>()
//            };
//        }
//    }
//}
