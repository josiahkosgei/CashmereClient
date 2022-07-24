using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IDeviceStatusRepository : IAsyncRepository<DeviceStatus>
    {
        public Task<DeviceStatus> GetByDeviceId(Guid DeviceId);
        public Task<DeviceStatus> GetByMachineName(string MachineName);
        public Task<IList<DeviceStatus>> GetAllAsync();
    }

}
