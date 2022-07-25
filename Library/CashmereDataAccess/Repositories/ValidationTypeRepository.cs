using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ValidationTypeRepository : RepositoryBase<ValidationType>, IValidationTypeRepository
    {
        public ValidationTypeRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}