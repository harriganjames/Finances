using System.Data.Entity.Validation;

namespace Finances.Persistence.EF.Interfaces
{
    public interface IEntityValidationHandler
    {
        void HandleDbEntityValidationException(DbEntityValidationException e);
        string ComposeErrorMessage(DbEntityValidationException e);
    }
}
