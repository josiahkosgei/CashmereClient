using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DeviceStatusRepository : RepositoryBase<DeviceStatus>, IDeviceStatusRepository
    {
        public DeviceStatusRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<DeviceStatus> GetByDeviceId(Guid DeviceId)
        {
            return await DbContext.DeviceStatus.Where(y => y.DeviceId == DeviceId).FirstOrDefaultAsync();
        }
        public async Task<DeviceStatus> GetByMachineName(string MachineName)
        {
            return await DbContext.DeviceStatus.Where(y => y.MachineName == MachineName).FirstOrDefaultAsync();
        }

        public async Task<IList<DeviceStatus>> GetAllAsync()
        {
            return await DbContext.DeviceStatus.ToListAsync();
        }
    }
}
