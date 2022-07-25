using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ValidationListValidationItemRepository : RepositoryBase<ValidationListValidationItem>, IValidationListValidationItemRepository
    {
        public ValidationListValidationItemRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}