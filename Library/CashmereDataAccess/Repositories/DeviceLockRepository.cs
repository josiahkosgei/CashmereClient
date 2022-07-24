using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DeviceLockRepository : RepositoryBase<DeviceLock>, IDeviceLockRepository
    {
        public DeviceLockRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<DeviceLock> GetFirst()
        {
            return await DbContext.DeviceLocks.OrderByDescending(x => x.LockDate).FirstOrDefaultAsync();
        }
    }
}
