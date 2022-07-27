using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IDeviceConfigRepository : IAsyncRepository<DeviceConfig>
    {
        public IList<DeviceConfig> ExecuteStoredProc(int config_group);
    }
}