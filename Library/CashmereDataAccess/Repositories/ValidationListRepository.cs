using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ValidationListRepository : RepositoryBase<ValidationList>, IValidationListRepository
    {
        public ValidationListRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}