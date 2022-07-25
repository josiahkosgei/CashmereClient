using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DeviceConfigRepository : RepositoryBase<DeviceConfig>, IDeviceConfigRepository
    {
        public DeviceConfigRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<IList<DeviceConfig>> ExecuteStoredProc(int config_group)
        {
            return await DbContext.DeviceConfigs.FromSqlRaw("EXECUTE  dbo.GetDeviceConfigByUserGroup @ConfigGroup = {0}", config_group).ToListAsync();
        }
    }
}