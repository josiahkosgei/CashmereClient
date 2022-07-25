using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ValidationTextRepository : RepositoryBase<ValidationText>, IValidationTextRepository
    {
        public ValidationTextRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}