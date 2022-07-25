using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class SessionExceptionRepository : RepositoryBase<SessionException>, ISessionExceptionRepository
    {
        public SessionExceptionRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}