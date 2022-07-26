using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class UptimeComponentStateRepository : RepositoryBase<UptimeComponentState>, IUptimeComponentStateRepository
    {
        public UptimeComponentStateRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<UptimeComponentState> GetByDeviceIdAsync(Guid deviceId, int state)
        {
            var result = depositorDBContext.UptimeComponentStates.Where(x => x.Device == deviceId && x.ComponentState == state && !x.EndDate.HasValue).OrderByDescending(x => x.Created).FirstOrDefault();
            return await Task.Run<UptimeComponentState>(() => result);
        }
        public async Task<List<UptimeComponentState>> GetEndDateHasValueAsync()
        {
            var result = depositorDBContext.UptimeComponentStates.Where(x => !x.EndDate.HasValue).ToList();
            return await Task.Run<List<UptimeComponentState>>(() => result);
        }
    }
}