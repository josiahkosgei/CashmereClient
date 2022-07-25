using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class UptimeModeRepository : RepositoryBase<UptimeMode>, IUptimeModeRepository
    {
        public UptimeModeRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<UptimeMode> GetByDeviceIdAsync(Guid deviceId)
        {
            return await DbContext.UptimeModes.Where(x =>x.Device ==deviceId).OrderByDescending(x => x.Created).FirstOrDefaultAsync();
        }

        public async Task<List<UptimeMode>> GetEndDateHasValueAsync()
        {
           return await DbContext.UptimeModes.Where(x => !x.EndDate.HasValue).ToListAsync();
        }
    }
}