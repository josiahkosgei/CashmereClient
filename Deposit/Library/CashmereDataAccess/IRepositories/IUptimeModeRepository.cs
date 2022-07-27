using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IUptimeModeRepository : IAsyncRepository<UptimeMode>
    {
        List<UptimeMode> GetEndDateHasValueAsync();
        UptimeMode GetByDeviceIdAsync(Guid deviceId);
    }
}