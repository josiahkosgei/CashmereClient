using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class UserLockRepository : RepositoryBase<UserLock>, IUserLockRepository
    {
        public UserLockRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public UserLock GetFirst()
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.UserLocks.OrderByDescending(x => x.LogDate).FirstOrDefault();
                return result;

            }
        }
    }
}
