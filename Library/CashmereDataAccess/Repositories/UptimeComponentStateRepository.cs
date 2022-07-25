using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class UptimeComponentStateRepository : RepositoryBase<UptimeComponentState>, IUptimeComponentStateRepository
    {
        public UptimeComponentStateRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
         public async Task<UptimeComponentState> GetByDeviceIdAsync(Guid deviceId, int state)
        {
            return await DbContext.UptimeComponentStates.Where(x =>x.Device ==deviceId  && x.ComponentState ==  state && !x.EndDate.HasValue).OrderByDescending(x => x.Created).FirstOrDefaultAsync();
        }
         public async Task<List<UptimeComponentState>> GetEndDateHasValueAsync()
        {
           return await DbContext.UptimeComponentStates.Where(x => !x.EndDate.HasValue).ToListAsync();
        }
    }
}