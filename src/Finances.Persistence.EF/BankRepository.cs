using System;
using System.Linq;
using System.Collections.Generic;
using Finances.Core.Interfaces;
//using Finances.Core.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Finances.Persistence.EF
{
    public class BankRepository : IBankRepository, IRepository
    {
        private readonly IModelContextFactory factory;
        private readonly IMappingEngine mapper;

        public BankRepository(
                        IModelContextFactory factory, 
                        IMappingEngine mapper)
        {
            this.factory = factory;
            this.mapper = mapper;
        }


        public int Add(Core.Entities.Bank entity)
        {
            var ef = mapper.Map<Bank>(entity);
            CommonRepository.Add<Bank>(factory,ef);
            mapper.Map<Bank, Core.Entities.Bank>(ef, entity);
            return ef.BankId;
        }

        //public int Add_old2(Core.Entities.Bank entity)
        //{
        //    var ef = Mapper.Map<Core.Entities.Bank, Bank>(entity);
        //    this.commonRepository.Add<Bank>(ef);
        //    Mapper.Map<Bank, Core.Entities.Bank>(ef, entity);
        //    return ef.BankId;
        //}

        //public bool Add_OLD(Core.Entities.Bank entity)
        //{
        //    using (FinanceEntities context = factory.CreateContext())
        //    {
        //        var ef = Mapper.Map<Core.Entities.Bank, Bank>(entity); 
        //        context.Entry(ef).State = System.Data.EntityState.Added;
        //        context.SaveChanges();
        //        Mapper.Map<Bank, Core.Entities.Bank>(ef, entity);
        //    }
        //    return true;
        //}

        public bool Update(Core.Entities.Bank entity)
        {
            using (FinanceEntities context = factory.CreateContext())
            {
                var ef = mapper.Map<Bank>(entity);
                context.Entry(ef).State = System.Data.EntityState.Modified;
                context.SaveChanges();
            }
            return true;
        }

        public bool Delete(Core.Entities.Bank entity)
        {
            using (FinanceEntities context = factory.CreateContext())
            {
                var ef = mapper.Map<Bank>(entity);
                context.Entry(ef).State = System.Data.EntityState.Deleted;
                context.SaveChanges();
            }
            return true;
        }

        public Core.Entities.Bank Read(int id)
        {
            using (FinanceEntities context = factory.CreateContext())
            {
                return mapper.Map<Core.Entities.Bank>(context.Banks.Where(bn => bn.BankId == id).FirstOrDefault());
            }
        }

        public List<Core.Entities.Bank> ReadList()
        {
            using (FinanceEntities context = factory.CreateContext())
            {
                return (from b in context.Banks
                        select b).Project(mapper).To<Core.Entities.Bank>().ToList();
            }
        }

        public List<Core.Entities.DataIdName> ReadListDataIdName()
        {
            using (FinanceEntities context = factory.CreateContext())
            {
                return context.Banks.Project(mapper).To<Core.Entities.DataIdName>().ToList();
            }
        }


    }
}
