using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DepositorSessionRepository : RepositoryBase<DepositorSession>, IDepositorSessionRepository
    {
        public DepositorSessionRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<DepositorSession> GetFirst()
        {
            var result = depositorDBContext.DepositorSessions.OrderByDescending(x => x.SessionEnd).FirstOrDefault();
            return await Task.Run<DepositorSession>(() => result);
        }
        public async Task<IList<DepositorSession>> GetCompleted()
        {
            var result = depositorDBContext.DepositorSessions.Where(x => x.Complete).ToList();
            return await Task.Run<IList<DepositorSession>>(() => result);
        }
    }
}
