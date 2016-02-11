using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Finances.Core.Interfaces;
using Finances.Core;

namespace Finances.Persistence.EF
{
    public class BankAccountRepository : IBankAccountRepository
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
            var ef = mapper.Map<BankAccount>(entity);
            using (FinanceEntities context = factory.CreateContext())
            {
                context.Entry(ef).State = EntityState.Added;
                context.SaveChanges();
            }
            //read back columns which may have changed
            entity.BankAccountId = ef.BankAccountId;
            entity.RecordCreatedDateTime = ef.RecordCreatedDateTime;
            entity.RecordUpdatedDateTime = ef.RecordUpdatedDateTime;
            id = ef.BankAccountId;
            return id;
        }

        public bool Update(Core.Entities.BankAccount entity)
        {
            var ef = mapper.Map<BankAccount>(entity);
            using (FinanceEntities context = factory.CreateContext())
            {
                context.Entry(ef).State = EntityState.Modified;
                context.SaveChanges();
            }
            //read back columns which may have changed
            entity.RecordUpdatedDateTime = ef.RecordUpdatedDateTime;
            return true;
        }

        public bool Delete(Core.Entities.BankAccount entity)
        {
            var ef = mapper.Map<BankAccount>(entity);
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
                    var ef = new BankAccount() { BankAccountId = id };
                    context.Entry(ef).State = EntityState.Deleted;
                }
                context.SaveChanges();
            }
            return true;
        }


        public Core.Entities.BankAccount Read(int id)
        {
            Core.Entities.BankAccount entity = null;
            using (FinanceEntities context = factory.CreateContext())
            {
                var ef = (from b in context.BankAccounts.Include(a => a.Bank)
                            where b.BankAccountId==id
                            select b).FirstOrDefault();

                entity = mapper.Map<Core.Entities.BankAccount>(ef);
            }
            return entity;
        }

        public List<Core.Entities.BankAccount> ReadList()
        {
            List<Core.Entities.BankAccount> list = null;
            using (FinanceEntities context = factory.CreateContext())
            {
                var ef = (from b in context.BankAccounts.Include(a => a.Bank)
                            select b).ToList();

                list = mapper.Map<List<Core.Entities.BankAccount>>(ef);
            }
            return list;
        }

        public async Task PostList(ITargetBlock<Core.Entities.BankAccount> target)
        {
            Diag.ThreadPrint("PostList - start");

            var transform = new TransformBlock<BankAccount, Core.Entities.BankAccount>(ef =>
                 mapper.Map<Core.Entities.BankAccount>(ef), new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 4 });

            transform.LinkTo(target);

            await Task.Run(() => 
            {
                Diag.ThreadPrint("PostList - task start");

                using (FinanceEntities context = factory.CreateContext())
                {
                    (from b in context.BankAccounts.Include(a => a.Bank)
                     select b).AsParallel().ForAll(ef => transform.Post(ef));
                    transform.Complete();
                    //await transform.Completion;
                    transform.Completion.ContinueWith(t =>
                    {
                        if (t.IsFaulted) target.Fault(t.Exception);
                        else
                        {
                            Diag.ThreadPrint("PostList - task set target complete");
                            target.Complete();
                        }
                    });
                }
                Diag.ThreadPrint("PostList - task end");
            });

            Diag.ThreadPrint("PostList - end");
        }

        public List<Core.Entities.BankAccount> ReadListByBankId(int bankId)
        {
            List<Core.Entities.BankAccount> list = null;
            using (FinanceEntities context = factory.CreateContext())
            {
                var ef = (from b in context.BankAccounts.Include(a => a.Bank)
                            where b.BankId == bankId
                            select b).ToList();

                list = mapper.Map<List<Core.Entities.BankAccount>>(ef);
            }
            return list;
        }

        public List<Core.Entities.DataIdName> ReadListDataIdName()
        {
            List<Core.Entities.DataIdName> list = null;
            using (FinanceEntities context = factory.CreateContext())
            {
                list = (from b in context.BankAccounts.Include(a => a.Bank)
                        select b).Project(mapper).To<Core.Entities.DataIdName>().ToList();

            }
            return list;
        }

    }
}
