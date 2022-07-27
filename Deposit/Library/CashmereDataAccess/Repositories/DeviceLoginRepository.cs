using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DeviceLoginRepository : RepositoryBase<DeviceLogin>, IDeviceLoginRepository
    {
        public DeviceLoginRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public DeviceLogin GetFirst(Guid Id)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.DeviceLogins.Where(x => x.User == Id && x.Success == true).OrderByDescending(x => x.LoginDate).FirstOrDefault();
                return result;

            }
        }
    }
}
