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
        public  UptimeComponentState GetByDeviceIdAsync(Guid deviceId, int state)
        {
            var result = _depositorDBContext.UptimeComponentStates.Where(x => x.Device == deviceId && x.ComponentState == state && !x.EndDate.HasValue).OrderByDescending(x => x.Created).FirstOrDefault();
            return result;
        }
        public  List<UptimeComponentState> GetEndDateHasValueAsync()
        {
            var result = _depositorDBContext.UptimeComponentStates.Where(x => !x.EndDate.HasValue).ToList();
            return result;
        }
    }
}