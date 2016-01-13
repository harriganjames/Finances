//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Finances.Core.Entities;
//using Finances.Core.Interfaces;
//using Finances.Interface;

//namespace Finances.Persistence.EF
//{
//    public class RepositoryWriteExceptionHandlerDecorator<TEntity> : IRepositoryWrite<TEntity> where TEntity : Entity
//    {
//        readonly IRepositoryWrite<TEntity> decorated;
//        readonly IExceptionService exceptionService;

//        public RepositoryWriteExceptionHandlerDecorator(
//                        IRepositoryWrite<TEntity> decorated,
//                        IExceptionService exceptionService
//                        )
//        {
//            this.decorated = decorated;
//            this.exceptionService = exceptionService;
//        }


//        public int Add(TEntity data)
//        {
//            int rv = 0;
//            try
//            {
//                rv = decorated.Add(data);
//            }
//            catch (Exception e)
//            {
//                exceptionService.ShowException(e);
//            }
//            return rv;
//        }

//        public bool Update(TEntity data)
//        {
//            bool rv = false;
//            try
//            {
//                rv = decorated.Update(data);
//            }
//            catch (Exception e)
//            {
//                exceptionService.ShowException(e);
//            }
//            return rv;
//        }

//        public bool Delete(TEntity data)
//        {
//            bool rv = false;
//            try
//            {
//                rv = decorated.Delete(data);
//            }
//            catch (Exception e)
//            {
//                exceptionService.ShowException(e);
//            }
//            return rv;
//        }

//        public bool Delete(List<int> ids)
//        {
//            bool rv = false;
//            try
//            {
//                rv = decorated.Delete(ids);
//            }
//            catch (Exception e)
//            {
//                exceptionService.ShowException(e);
//            }
//            return rv;
//        }
//    }
//}
