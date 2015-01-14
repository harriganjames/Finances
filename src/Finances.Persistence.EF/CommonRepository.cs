using System;
using System.Linq;
using System.Collections.Generic;
using Finances.Core.Interfaces;
using Finances.Core.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Data.Entity.Validation;
using System.Text;

namespace Finances.Persistence.EF
{
    internal static class CommonRepository
    {
        //private readonly IModelContextFactory factory;

        //public CommonRepository(IModelContextFactory factory)
        //{
        //    this.factory = factory;
        //}

        //public bool Add<TOrmEntity>(TOrmEntity ef) where TOrmEntity : class
        //{
        //    using (FinanceEntities context = factory.CreateContext())
        //    {
        //        context.Entry(ef).State = System.Data.EntityState.Added;
        //        context.SaveChanges();
        //    }
        //    return true;
        //}


        internal static bool Add<T>(IModelContextFactory factory,T ef) where T : class
        {
            using (FinanceEntities context = factory.CreateContext())
            {
                context.Entry(ef).State = System.Data.EntityState.Added;
                context.SaveChanges();
            }
            return true;
        }

        internal static void HandleDbEntityValidationException(DbEntityValidationException e)
        {
            var message = new StringBuilder();

            foreach (var err in e.EntityValidationErrors)
            {
                foreach (var inner in err.ValidationErrors)
                {
                    message.AppendLine(inner.ErrorMessage);
                }
            }
            throw new Exception(message.ToString());
        }


    }
}
