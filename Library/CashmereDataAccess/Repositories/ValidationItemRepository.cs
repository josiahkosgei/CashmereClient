using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ValidationItemRepository : RepositoryBase<ValidationItem>, IValidationItemRepository
    {
        public ValidationItemRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}