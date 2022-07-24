using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class UserLockRepository : RepositoryBase<UserLock>, IUserLockRepository
    {
        public UserLockRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<UserLock> GetFirst()
        {
            return await DbContext.UserLocks.OrderByDescending(x => x.LogDate).FirstOrDefaultAsync();
        }
    }
}
