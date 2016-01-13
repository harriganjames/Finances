//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using Finances.Core.Entities;
//using Finances.Core.Interfaces;

//namespace Finances.Persistence.EF
//{
//    public class RepositoryWriteDecoratorTest<TEntity> : IRepositoryWrite<TEntity> where TEntity : Entity
//    {
//        readonly IRepositoryWrite<TEntity> decorated;

//        public RepositoryWriteDecoratorTest(IRepositoryWrite<TEntity> decorated)
//        {
//            this.decorated = decorated;
//        }

//        public int Add(TEntity data)
//        {
//            return decorated.Add(data);
//        }

//        public bool Update(TEntity data)
//        {
//            return decorated.Update(data);
//        }

//        public bool Delete(TEntity data)
//        {
//            return decorated.Delete(data);
//        }

//        public bool Delete(List<int> ids)
//        {
//            return decorated.Delete(ids);
//        }
//    }
//}
