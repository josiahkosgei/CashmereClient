using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ApplicationLogRepository : RepositoryBase<ApplicationLog>, IApplicationLogRepository
    {
        public ApplicationLogRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}
