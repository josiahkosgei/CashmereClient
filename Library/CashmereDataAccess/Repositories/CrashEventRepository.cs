using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class CrashEventRepository : RepositoryBase<CrashEvent>, ICrashEventRepository
    {
        public CrashEventRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}