using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IUptimeModeRepository : IAsyncRepository<UptimeMode>
    {
        Task<List<UptimeMode>> GetEndDateHasValueAsync();
        Task<UptimeMode> GetByDeviceIdAsync(Guid deviceId);
    }
}