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

        public async Task<UptimeMode> GetByDeviceIdAsync(Guid deviceId)
        {
            var result = depositorDBContext.UptimeModes.Where(x => x.Device == deviceId).OrderByDescending(x => x.Created).FirstOrDefault();
            return await Task.Run<UptimeMode>(() => result);
        }

        public async Task<List<UptimeMode>> GetEndDateHasValueAsync()
        {
            var result = depositorDBContext.UptimeModes.Where(x => !x.EndDate.HasValue).ToList();
            return await Task.Run<List<UptimeMode>>(() => result);
        }
    }
}