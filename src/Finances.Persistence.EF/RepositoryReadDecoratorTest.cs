//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Finances.Core.Entities;
//using Finances.Core.Interfaces;

//namespace Finances.Persistence.EF
//{
//    public class RepositoryReadDecoratorTest<TEntity> : IRepositoryRead<TEntity>, IRepository where TEntity : Entity
//    {
//        readonly IRepositoryRead<TEntity> decorated;

//        public RepositoryReadDecoratorTest(IRepositoryRead<TEntity> decorated)
//        {
//            this.decorated = decorated;
//        }

//        public TEntity Read(int dataId)
//        {
//            return decorated.Read(dataId);
//        }

//        public List<TEntity> ReadList()
//        {
//            return decorated.ReadList();
//        }

//        public List<DataIdName> ReadListDataIdName()
//        {
//            return decorated.ReadListDataIdName();
//        }
//    }
//}
