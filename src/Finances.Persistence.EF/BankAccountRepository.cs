﻿using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using Finances.Core.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Data.Entity.Validation;
using System.Text;

namespace Finances.Persistence.EF
{
    public class BankAccountRepository : IBankAccountRepository, IRepository
    {
        private readonly IModelContextFactory factory;
        private readonly IMappingEngine mapper;

        public BankAccountRepository(
                        IModelContextFactory factory, 
                        IMappingEngine mapper)
        {
            this.factory = factory;
            this.mapper = mapper;
        }


        public int Add(Core.Entities.BankAccount entity)
        {
            int id=0;
            try
            {
                var ef = mapper.Map<BankAccount>(entity);
                using (FinanceEntities context = factory.CreateContext())
                {
                    context.Entry(ef).State = System.Data.EntityState.Added;
                    context.SaveChanges();
                }
                //read back columns which may have changed
                entity.BankAccountId = ef.BankAccountId;
                entity.RecordCreatedDateTime = ef.RecordCreatedDateTime;
                entity.RecordUpdatedDateTime = ef.RecordUpdatedDateTime;
                id = ef.BankAccountId;
            }
            catch (DbEntityValidationException e)
            {
                CommonRepository.HandleDbEntityValidationException(e);
            }
            return id;
        }

        public bool Update(Core.Entities.BankAccount entity)
        {
            try
            {
                var ef = mapper.Map<BankAccount>(entity);
                using (FinanceEntities context = factory.CreateContext())
                {
                    context.Entry(ef).State = System.Data.EntityState.Modified;
                    context.SaveChanges();
                }
                //read back columns which may have changed
                entity.RecordUpdatedDateTime = ef.RecordUpdatedDateTime;
            }
            catch (DbEntityValidationException e)
            {
                CommonRepository.HandleDbEntityValidationException(e);
            }
            return true;
        }

        public bool Delete(Core.Entities.BankAccount entity)
        {
            try
            {
                var ef = mapper.Map<BankAccount>(entity);
                using (FinanceEntities context = factory.CreateContext())
                {
                    context.Entry(ef).State = System.Data.EntityState.Deleted;
                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                CommonRepository.HandleDbEntityValidationException(e);
            }
            return true;
        }

        public Core.Entities.BankAccount Read(int id)
        {
            Core.Entities.BankAccount entity = null;
            try
            {
                using (FinanceEntities context = factory.CreateContext())
                {
                    var ef = (from b in context.BankAccounts.Include(a => a.Bank)
                              where b.BankAccountId==id
                              select b).FirstOrDefault();

                    entity = mapper.Map<Core.Entities.BankAccount>(ef);
                }
            }
            catch (DbEntityValidationException e)
            {
                CommonRepository.HandleDbEntityValidationException(e);
            }
            return entity;
        }

        public List<Core.Entities.BankAccount> ReadList()
        {
            List<Core.Entities.BankAccount> list = null;
            try
            {
                using (FinanceEntities context = factory.CreateContext())
                {
                    var ef = (from b in context.BankAccounts.Include(a => a.Bank)
                              select b).ToList();

                    list = mapper.Map<List<Core.Entities.BankAccount>>(ef);

                    //var x = (from b in context.BankAccounts.Include(ba=>ba.Bank)
                    //        select b).Project(mapper).To<Core.Entities.BankAccount>().ToList();
                }
            }
            catch (DbEntityValidationException e)
            {
                CommonRepository.HandleDbEntityValidationException(e);
            }
            return list;
        }

        public List<Core.Entities.BankAccount> ReadListByBankId(int bankId)
        {
            List<Core.Entities.BankAccount> list = null;
            try
            {
                using (FinanceEntities context = factory.CreateContext())
                {
                    var ef = (from b in context.BankAccounts.Include(a => a.Bank)
                              where b.BankId == bankId
                              select b).ToList();

                    // how to include Banks?
                    //var ef = context.BankAccounts.SqlQuery("select * from dbo.BankAccount where BankId={0}", bankId).ToList();

                    list = mapper.Map<List<Core.Entities.BankAccount>>(ef);
                }
            }
            catch (DbEntityValidationException e)
            {
                CommonRepository.HandleDbEntityValidationException(e);
            }
            return list;
        }

        public List<Core.Entities.DataIdName> ReadListDataIdName()
        {
            List<Core.Entities.DataIdName> list = null;
            try
            {
                using (FinanceEntities context = factory.CreateContext())
                {
                    var ef = (from b in context.BankAccounts.Include(a => a.Bank)
                              select b).ToList();

                    list = mapper.Map<List<Core.Entities.DataIdName>>(ef);
                }
            }
            catch (DbEntityValidationException e)
            {
                CommonRepository.HandleDbEntityValidationException(e);
            }
            return list;
        }

        #region Privates

        //private void SetChildEntitiesState(FinanceEntities context, BankAccount ef)
        //{
        //    if (ef.Bank != null)
        //    {
        //        context.Entry(ef.Bank).State = System.Data.EntityState.Unchanged;
        //    }
        //}

        #endregion


    }
}
