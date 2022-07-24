using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DeviceLoginRepository : RepositoryBase<DeviceLogin>, IDeviceLoginRepository
    {
        public DeviceLoginRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<DeviceLogin> GetFirst(Guid Id)
        {
            return await DbContext.DeviceLogins.Where(x => x.User == Id && x.Success == true).OrderByDescending(x => x.LoginDate).FirstOrDefaultAsync();
        }
    }
}
