using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DepositorSessionRepository : RepositoryBase<DepositorSession>, IDepositorSessionRepository
    {
        public DepositorSessionRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<DepositorSession> GetFirst()
        {
            return await DbContext.DepositorSessions.OrderByDescending(x => x.SessionEnd).FirstOrDefaultAsync();
        }
        public async Task<IList<DepositorSession>> GetCompleted()
        {
            return await DbContext.DepositorSessions.Where(x => x.Complete).ToListAsync();
        }
    }
}
