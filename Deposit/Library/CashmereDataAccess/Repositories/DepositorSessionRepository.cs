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

        public  DepositorSession GetFirst()
        {
            var result = _depositorDBContext.DepositorSessions.OrderByDescending(x => x.SessionEnd).FirstOrDefault();
            return result;
        }
        public  IList<DepositorSession> GetCompleted()
        {
            var result = _depositorDBContext.DepositorSessions.Where(x => !x.Complete).ToList();
            return result;
        }
    }
}
