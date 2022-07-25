using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IUptimeComponentStateRepository : IAsyncRepository<UptimeComponentState>
    {
        Task<UptimeComponentState> GetByDeviceIdAsync(Guid deviceId, int state);
        Task<List<UptimeComponentState>> GetEndDateHasValueAsync();
    }
}