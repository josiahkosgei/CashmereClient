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

        public  DeviceLock GetFirst()
        {
            var result = _depositorDBContext.DeviceLocks.OrderByDescending(x => x.LockDate).FirstOrDefault();
            return result;
        }
    }
}
