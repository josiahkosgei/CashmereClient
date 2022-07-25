using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IDeviceConfigRepository : IAsyncRepository<DeviceConfig>
    {
        public Task<IList<DeviceConfig>> ExecuteStoredProc(int config_group);
    }
}