using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IUptimeComponentStateRepository : IAsyncRepository<UptimeComponentState>
    {
        UptimeComponentState GetByDeviceIdAsync(Guid deviceId, int state);
        List<UptimeComponentState> GetEndDateHasValueAsync();
    }
}