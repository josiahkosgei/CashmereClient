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

        public DepositorSession GetFirst()
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.DepositorSessions.OrderByDescending(x => x.SessionEnd).FirstOrDefault();
                return result;

            }
        }
        public IList<DepositorSession> GetCompleted()
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.DepositorSessions.Where(x => !x.Complete).ToList();
                return result;

            }
        }
    }
}
