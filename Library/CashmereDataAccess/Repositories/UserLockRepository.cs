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

        public async Task<UserLock> GetFirst()
        {
            var result = depositorDBContext.UserLocks.OrderByDescending(x => x.LogDate).FirstOrDefault();
            return await Task.Run<UserLock>(() => result);
        }
    }
}
