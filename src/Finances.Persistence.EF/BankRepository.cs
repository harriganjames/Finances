using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Data.Entity.Validation;

using AutoMapper;
using AutoMapper.QueryableExtensions;

//using Finances.Interface;
using Finances.Core.Interfaces;

namespace Finances.Persistence.EF
{
    public class BankRepository : IBankRepository
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
            int id = 0;

            var ef = mapper.Map<Bank>(entity);
            using (FinanceEntities context = factory.CreateContext())
            {
                context.Entry(ef).State = EntityState.Added;
                context.SaveChanges();
            }
            //read back columns which may have changed
            entity.BankId = ef.BankId;
            //entity.RecordCreatedDateTime = ef.RecordCreatedDateTime;
            //entity.RecordUpdatedDateTime = ef.RecordUpdatedDateTime;
            id = ef.BankId;

            return id;
        }


        public bool Update(Core.Entities.Bank entity)
        {
            var ef = mapper.Map<Bank>(entity);
            using (FinanceEntities context = factory.CreateContext())
            {
                context.Entry(ef).State = EntityState.Modified;
                context.SaveChanges();
            }
            //read back columns which may have changed
            //entity.RecordUpdatedDateTime = ef.RecordUpdatedDateTime;

            return true;
        }


        public bool Delete(Core.Entities.Bank entity)
        {
            var ef = mapper.Map<Bank>(entity);
            using (FinanceEntities context = factory.CreateContext())
            {
                context.Entry(ef).State = EntityState.Deleted;
                context.SaveChanges();
            }

            return true;
        }


        public bool Delete(List<int> ids)
        {
            using (FinanceEntities context = factory.CreateContext())
            {
                foreach (var id in ids)
                {
                    var ef = new Bank() { BankId = id };
                    context.Entry(ef).State = EntityState.Deleted;
                }
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
