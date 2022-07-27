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

        public  UptimeMode GetByDeviceIdAsync(Guid deviceId)
        {
            var result = _depositorDBContext.UptimeModes.Where(x => x.Device == deviceId).OrderByDescending(x => x.Created).FirstOrDefault();
            return result;
        }

        public  List<UptimeMode> GetEndDateHasValueAsync()
        {
            var result = _depositorDBContext.UptimeModes.Where(x => !x.EndDate.HasValue).ToList();
            return result;
        }
    }
}