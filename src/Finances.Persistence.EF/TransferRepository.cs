using System;
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
    public class TransferRepository : ITransferRepository
    {
        private readonly IModelContextFactory factory;
        private readonly IMappingEngine mapper;

        public TransferRepository(
                        IModelContextFactory factory, 
                        IMappingEngine mapper)
        {
            this.factory = factory;
            this.mapper = mapper;
        }


        public int Add(Core.Entities.Transfer entity)
        {
            int id=0;
            try
            {
                var ef = mapper.Map<Transfer>(entity);
                using (FinanceEntities context = factory.CreateContext())
                {
                    context.Entry(ef).State = System.Data.EntityState.Added;
                    SetChildEntitiesState(context, ef);
                    context.SaveChanges();
                }
                mapper.Map<Transfer, Core.Entities.Transfer>(ef, entity);
                //entity.BankAccountId = ef.BankAccountId;
                id = ef.TransferId;
            }
            catch (DbEntityValidationException e)
            {
                CommonRepository.HandleDbEntityValidationException(e);
            }
            return id;
        }

        public bool Update(Core.Entities.Transfer entity)
        {
            try
            {
                var ef = mapper.Map<Transfer>(entity);
                using (FinanceEntities context = factory.CreateContext())
                {
                    context.Entry(ef).State = System.Data.EntityState.Modified;
                    SetChildEntitiesState(context, ef);
                    context.SaveChanges();
                }
                mapper.Map<Transfer, Core.Entities.Transfer>(ef, entity);
            }
            catch (DbEntityValidationException e)
            {
                CommonRepository.HandleDbEntityValidationException(e);
            }
            return true;
        }


        private void SetChildEntitiesState(FinanceEntities context, Transfer ef)
        {
            if (ef.FromBankAccount != null)
            {
                context.Entry(ef.FromBankAccount).State = System.Data.EntityState.Unchanged;
                if (ef.FromBankAccount.Bank != null)
                {
                    context.Entry(ef.FromBankAccount.Bank).State = System.Data.EntityState.Detached;
                }
            }
            if (ef.ToBankAccount != null)
            {
                context.Entry(ef.ToBankAccount).State = System.Data.EntityState.Unchanged;
                if (ef.ToBankAccount.Bank != null)
                {
                    context.Entry(ef.ToBankAccount.Bank).State = System.Data.EntityState.Detached;
                }
            }
        }

        public bool Delete(Core.Entities.Transfer entity)
        {
            try
            {
                var ef = mapper.Map<Transfer>(entity);
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

        public Core.Entities.Transfer Read(int id)
        {
            Core.Entities.Transfer entity = null;
            try
            {
                using (FinanceEntities context = factory.CreateContext())
                {
                    var ef = (from b in context.Transfers
                                    .Include(a => a.FromBankAccount)
                                    .Include(a => a.FromBankAccount.Bank)
                                    .Include(a => a.ToBankAccount)
                                    .Include(a => a.ToBankAccount.Bank)
                              where b.TransferId==id
                              select b).FirstOrDefault();

                    entity = mapper.Map<Core.Entities.Transfer>(ef);
                }
            }
            catch (DbEntityValidationException e)
            {
                CommonRepository.HandleDbEntityValidationException(e);
            }
            return entity;
        }

        public List<Core.Entities.Transfer> ReadList()
        {
            List<Core.Entities.Transfer> list = null;
            try
            {
                using (FinanceEntities context = factory.CreateContext())
                {
                    var ef = (from b in context.Transfers
                                    .Include(a => a.FromBankAccount)
                                    .Include(a => a.FromBankAccount.Bank)
                                    .Include(a => a.ToBankAccount)
                                    .Include(a => a.ToBankAccount.Bank)
                              select b).ToList();

                    list = mapper.Map<List<Core.Entities.Transfer>>(ef);

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

        //public List<Core.Entities> ReadListByBankId(int bankId)
        //{
        //    List<Core.Entities.BankAccount> list = null;
        //    try
        //    {
        //        using (FinanceEntities context = factory.CreateContext())
        //        {
        //            var ef = (from b in context.BankAccounts.Include(a => a.Bank)
        //                      where b.BankId==bankId
        //                      select b).ToList();

        //            list = mapper.Map<List<Core.Entities.BankAccount>>(ef);
        //        }
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        CommonRepository.HandleDbEntityValidationException(e);
        //    }
        //    return list;
        //}

        public List<Core.Entities.DataIdName> ReadListDataIdName()
        {
            List<Core.Entities.DataIdName> list = null;
            try
            {
                using (FinanceEntities context = factory.CreateContext())
                {
                    var ef = (from b in context.Transfers
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


    }
}
