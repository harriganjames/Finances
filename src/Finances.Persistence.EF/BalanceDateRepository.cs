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
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Finances.Core;
//using EntityFramework.Extensions;


namespace Finances.Persistence.EF
{
    public class BalanceDateRepository : IBalanceDateRepository
    {
        private readonly IModelContextFactory factory;
        private readonly IMappingEngine mapper;

        public BalanceDateRepository(
                        IModelContextFactory factory, 
                        IMappingEngine mapper)
        {
            this.factory = factory;
            this.mapper = mapper;
        }


        public int Add(Core.Entities.BalanceDate entity)
        {
            int id=0;
            var ef = mapper.Map<BalanceDate>(entity);
            using (FinanceEntities context = factory.CreateContext())
            {
                context.Entry(ef).State = EntityState.Added;
                context.SaveChanges();
            }
            //read back columns which may have changed
            entity.BalanceDateId = ef.BalanceDateId;
            entity.RecordCreatedDateTime = ef.RecordCreatedDateTime;
            //entity.RecordUpdatedDateTime = ef.RecordUpdatedDateTime;

            int i = 0;
            foreach (var cba in ef.BalanceDateBankAccounts)
            {
                if (entity.BalanceDateBankAccounts.Count <= i)
                    break;

                var e = entity.BalanceDateBankAccounts[i];

                e.BalanceDateBankAccountId = cba.BalanceDateBankAccountId;
                e.RecordCreatedDateTime = cba.RecordCreatedDateTime;

                i++;
            }


            id = ef.BalanceDateId;
            return id;
        }

        public bool Update(Core.Entities.BalanceDate entity)
        {
            var ef = mapper.Map<BalanceDate>(entity);
            using (FinanceEntities context = factory.CreateContext())
            {

                // read in children
                var cbas = from b in context.BalanceDateBankAccounts
                           where b.BalanceDateId == ef.BalanceDateId
                           select b;

                foreach (var cba in cbas)
                {
                    if (ef.BalanceDateBankAccounts.Count(a => a.BalanceDateBankAccountId == cba.BalanceDateBankAccountId) == 0)
                        context.Entry(cba).State = EntityState.Deleted;
                    else
                        context.Entry(cba).State = EntityState.Detached;
                }

                foreach (var cba in ef.BalanceDateBankAccounts)
                {
                    if (cba.BalanceDateBankAccountId > 0)
                        context.Entry(cba).State = EntityState.Modified;
                    else
                        context.Entry(cba).State = EntityState.Added;
                }



                context.Entry(ef).State = EntityState.Modified;

                var s = ShowEntityStates(context);

                context.SaveChanges();
            }
            //read back data which may have changed
            //entity.RecordUpdatedDateTime = ef.RecordUpdatedDateTime;

            int i = 0;
            foreach (var cba in ef.BalanceDateBankAccounts)
            {
                if (entity.BalanceDateBankAccounts.Count <= i)
                    break;

                var e = entity.BalanceDateBankAccounts[i];

                e.BalanceDateBankAccountId = cba.BalanceDateBankAccountId;
                e.RecordCreatedDateTime = cba.RecordCreatedDateTime;

                i++;
            }

            return true;
        }


        public bool Delete(Core.Entities.BalanceDate entity)
        {
            var ef = mapper.Map<BalanceDate>(entity);
            //need to delete children to prevent EF FK error
            ef.BalanceDateBankAccounts.Clear();
            using (FinanceEntities context = factory.CreateContext())
            {
                context.Entry(ef).State = EntityState.Deleted;
                // deletes are cascaded to BalanceBankAccount
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
                    var ef = new BalanceDate() { BalanceDateId = id };
                    context.Entry(ef).State = EntityState.Deleted;
                }

                // deletes are cascaded to BalanceBankAccount
                context.SaveChanges();
            }
            return true;
        }

        private IQueryable<BalanceDate> ReadQuery(FinanceEntities context, int? id=null)
        {
            return (from b in context.BalanceDates
                                .Include(a => a.BalanceDateBankAccounts)
                                .Include("BalanceDateBankAccounts.BankAccount")
                                .Include("BalanceDateBankAccounts.BankAccount.Bank")
                    where !id.HasValue || b.BalanceDateId == id
                    select b);
        }


        public Core.Entities.BalanceDate Read(int id)
        {
            Core.Entities.BalanceDate entity = null;
            using (FinanceEntities context = factory.CreateContext())
            {
                var ef = ReadQuery(context, id).FirstOrDefault();

                //var ef = (from b in context.BalanceDates
                //                .Include(a => a.BalanceDateBankAccounts)
                //                .Include("BalanceDateBankAccounts.BankAccount")
                //                .Include("BalanceDateBankAccounts.BankAccount.Bank")
                //          where b.BalanceDateId==id
                //            select b).FirstOrDefault();

                entity = mapper.Map<Core.Entities.BalanceDate>(ef);
            }
            return entity;
        }

        public List<Core.Entities.BalanceDate> ReadList()
        {
            List<Core.Entities.BalanceDate> list = null;
            using (FinanceEntities context = factory.CreateContext())
            {
                var ef = ReadQuery(context).ToList();
                //var ef = (from b in context.BalanceDates
                //                .Include(a => a.BalanceDateBankAccounts)
                //                .Include("BalanceDateBankAccounts.BankAccount")
                //                .Include("BalanceDateBankAccounts.BankAccount.Bank")
                //          select b).ToList();

                list = mapper.Map<List<Core.Entities.BalanceDate>>(ef);
            }
            return list;
        }

        public async Task<List<Core.Entities.BalanceDate>> ReadListAsync()
        {
            return await Task.Factory.StartNew(() =>
               {
                   using (FinanceEntities context = factory.CreateContext())
                   {
                       var ef = ReadQuery(context).ToList();
                       return mapper.Map<List<Core.Entities.BalanceDate>>(ef);
                   }
               });
        }


        public async Task PostList(ITargetBlock<Core.Entities.BalanceDate> target)
        {
            Diag.ThreadPrint("PostList - start");

            var transform = new TransformBlock<BalanceDate, Core.Entities.BalanceDate>(ef =>
                 mapper.Map<Core.Entities.BalanceDate>(ef), new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 4 });

            transform.LinkTo(target,new DataflowLinkOptions() { PropagateCompletion = true });

            await Task.Run(() =>
            {
                Diag.ThreadPrint("PostList - task start");

                using (FinanceEntities context = factory.CreateContext())
                {
                    (from b in context.BalanceDates
                                .Include(a => a.BalanceDateBankAccounts)
                                .Include("BalanceDateBankAccounts.BankAccount")
                                .Include("BalanceDateBankAccounts.BankAccount.Bank")
                     select b).AsParallel().ForAll(ef => transform.Post(ef));
                    //await transform.Completion;
                    //transform.Completion.ContinueWith(t =>
                    //{
                    //    if (t.IsFaulted) target.Fault(t.Exception);
                    //    else
                    //    {
                    //        Diag.ThreadPrint("PostList - task set target complete");
                    //        target.Complete();
                    //    }
                    //});
                    transform.Complete();
                }
                Diag.ThreadPrint("PostList - task end");
            }).ConfigureAwait(false);

            Diag.ThreadPrint("PostList - end");
        }



        public List<Core.Entities.DataIdName> ReadListDataIdName()
        {
            List<Core.Entities.DataIdName> list = null;
            using (FinanceEntities context = factory.CreateContext())
            {
                var ef = (from b in context.BalanceDates
                        select b).ToList();

                list = mapper.Map<List<Core.Entities.DataIdName>>(ef);

                //list = (from b in context.BalanceDates
                //        select b).Project(mapper).To<Core.Entities.DataIdName>().ToList();
            }
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
