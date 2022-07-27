using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DeviceLockRepository : RepositoryBase<DeviceLock>, IDeviceLockRepository
    {
        public DeviceLockRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public DeviceLock GetFirst()
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.DeviceLocks.OrderByDescending(x => x.LockDate).FirstOrDefault();
                return result;

            }
        }
    }
}
