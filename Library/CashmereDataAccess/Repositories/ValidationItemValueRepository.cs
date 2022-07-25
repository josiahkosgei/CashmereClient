using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ValidationItemValueRepository : RepositoryBase<ValidationItemValue>, IValidationItemValueRepository
    {
        public ValidationItemValueRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}