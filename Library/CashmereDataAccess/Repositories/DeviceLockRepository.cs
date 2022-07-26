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

        public async Task<DeviceLock> GetFirst()
        {
            var result = depositorDBContext.DeviceLocks.OrderByDescending(x => x.LockDate).FirstOrDefault();
            return await Task.Run<DeviceLock>(() => result);
        }
    }
}
