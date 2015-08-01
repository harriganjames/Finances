using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using Finances.Core.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Data.Entity.Validation;
using System.Text;
using System.Data;
//using EntityFramework.Extensions;


namespace Finances.Persistence.EF
{
    public class CashflowRepository : ICashflowRepository, IRepositoryRead<Core.Entities.Cashflow>, IRepository
    {
        private readonly IModelContextFactory factory;
        private readonly IMappingEngine mapper;

        public CashflowRepository(
                        IModelContextFactory factory, 
                        IMappingEngine mapper)
        {
            this.factory = factory;
            this.mapper = mapper;
        }


        public int Add(Core.Entities.Cashflow entity)
        {
            int id=0;
            try
            {
                var ef = mapper.Map<Cashflow>(entity);
                using (FinanceEntities context = factory.CreateContext())
                {
                    context.Entry(ef).State = EntityState.Added;
                    context.SaveChanges();
                }
                //read back columns which may have changed
                entity.CashflowId = ef.CashflowId;
                entity.RecordCreatedDateTime = ef.RecordCreatedDateTime;
                entity.RecordUpdatedDateTime = ef.RecordUpdatedDateTime;

                int i = 0;
                foreach (var cba in ef.CashflowBankAccounts)
                {
                    if (entity.CashflowBankAccounts.Count <= i)
                        break;

                    var e = entity.CashflowBankAccounts[i];

                    e.CashflowBankAccountId = cba.CashflowBankAccountId;
                    e.RecordCreatedDateTime = cba.RecordCreatedDateTime;

                    i++;
                }

                id = ef.CashflowId;
            }
            catch (DbEntityValidationException e)
            {
                CommonRepository.HandleDbEntityValidationException(e);
            }
            return id;
        }

        public bool Update(Core.Entities.Cashflow entity)
        {
            try
            {
                var ef = mapper.Map<Cashflow>(entity);
                using (FinanceEntities context = factory.CreateContext())
                {
                    // read in children
                    var cbas = from b in context.CashflowBankAccounts
                              where b.CashflowId==ef.CashflowId
                              select b;
                    
                    foreach (var cba in cbas)
                    {
                        if(ef.CashflowBankAccounts.Count(a=>a.CashflowBankAccountId==cba.CashflowBankAccountId)==0)
                            context.Entry(cba).State = EntityState.Deleted;
                        else
                            context.Entry(cba).State = EntityState.Detached;
                    }

                    foreach (var cba in ef.CashflowBankAccounts)
                    {
                        if(cba.CashflowBankAccountId>0)
                            context.Entry(cba).State = EntityState.Modified;
                        else
                            context.Entry(cba).State = EntityState.Added;
                    }

                    context.Entry(ef).State = EntityState.Modified;

                    var s = ShowEntityStates(context);
                    context.SaveChanges();
                }
                //read back data which may have changed
                entity.RecordUpdatedDateTime = ef.RecordUpdatedDateTime;

                int i = 0;
                foreach (var cba in ef.CashflowBankAccounts)
                {
                    if (entity.CashflowBankAccounts.Count <= i)
                        break;

                    var e = entity.CashflowBankAccounts[i];

                    if (e.CashflowBankAccountId == 0)
                    {
                        e.CashflowBankAccountId = cba.CashflowBankAccountId;
                        e.RecordCreatedDateTime = cba.RecordCreatedDateTime;
                    }
                    i++;
                }

            }
            catch (DbEntityValidationException e)
            {
                CommonRepository.HandleDbEntityValidationException(e);
            }
            return true;
        }


        public bool Delete(Core.Entities.Cashflow entity)
        {
            try
            {
                var ef = mapper.Map<Cashflow>(entity);
                ef.CashflowBankAccounts.Clear();
                using (FinanceEntities context = factory.CreateContext())
                {
                    context.Entry(ef).State = EntityState.Deleted;
                    // deletes are cascaded to CashflowBankAccount
                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                CommonRepository.HandleDbEntityValidationException(e);
            }
            return true;
        }

        public bool Delete(List<int> ids)
        {
            try
            {
                using (FinanceEntities context = factory.CreateContext())
                {
                    //context.Cashflows.Delete(e=>ids.Contains(e.CashflowId));

                    foreach (var id in ids)
                    {
                        var ef = new Cashflow() { CashflowId = id };
                        context.Entry(ef).State = EntityState.Deleted;
                    }

                    // deletes are cascaded to CashflowBankAccount
                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                CommonRepository.HandleDbEntityValidationException(e);
            }
            return true;
        }



        public Core.Entities.Cashflow Read(int id)
        {
            Core.Entities.Cashflow entity = null;
            //try
            //{
                using (FinanceEntities context = factory.CreateContext())
                {
                    var ef = (from b in context.Cashflows
                                    .Include(a => a.CashflowBankAccounts)
                                    .Include("CashflowBankAccounts.BankAccount")
                                    .Include("CashflowBankAccounts.BankAccount.Bank")
                              where b.CashflowId==id
                              select b).FirstOrDefault();

                    entity = mapper.Map<Core.Entities.Cashflow>(ef);
                }
            //}
            //catch (DbEntityValidationException e)
            //{
            //    CommonRepository.HandleDbEntityValidationException(e);
            //}
            return entity;
        }

        public List<Core.Entities.Cashflow> ReadList()
        {
            List<Core.Entities.Cashflow> list = null;
            //try
            //{
                using (FinanceEntities context = factory.CreateContext())
                {
                    var ef = (from b in context.Cashflows
                                    .Include(a => a.CashflowBankAccounts)
                                    .Include("CashflowBankAccounts.BankAccount")
                                    .Include("CashflowBankAccounts.BankAccount.Bank")
                              select b).ToList();

                    list = mapper.Map<List<Core.Entities.Cashflow>>(ef);
                }
            //}
            //catch (DbEntityValidationException e)
            //{
            //    CommonRepository.HandleDbEntityValidationException(e);
            //}
            return list;
        }


        public List<Core.Entities.DataIdName> ReadListDataIdName()
        {
            List<Core.Entities.DataIdName> list = null;
            //try
            //{
                using (FinanceEntities context = factory.CreateContext())
                {
                    list = (from b in context.Cashflows
                            select b).Project(mapper).To<Core.Entities.DataIdName>().ToList();
                }
            //}
            //catch (DbEntityValidationException e)
            //{
            //    CommonRepository.HandleDbEntityValidationException(e);
            //}
            return list;
        }


        string ShowEntityStates(FinanceEntities context)
        {
            var sb = new StringBuilder();

            foreach (var entity in context.ChangeTracker.Entries())
            {
                sb.AppendFormat("{0}-{1}", entity.Entity.GetType().Name, entity.State.ToString());
                sb.AppendLine();
            }
            return sb.ToString();
        }

    }
}
