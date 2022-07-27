using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class UptimeModeRepository : RepositoryBase<UptimeMode>, IUptimeModeRepository
    {
        public UptimeModeRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public UptimeMode GetByDeviceIdAsync(Guid deviceId)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.UptimeModes.Where(x => x.Device == deviceId).OrderByDescending(x => x.Created).FirstOrDefault();
                return result;

            }
        }

        public List<UptimeMode> GetEndDateHasValueAsync()
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.UptimeModes.Where(x => !x.EndDate.HasValue).ToList();
                return result;

            }
        }
    }
}