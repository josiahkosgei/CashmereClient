using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IDeviceStatusRepository : IAsyncRepository<DeviceStatus>
    {
        public DeviceStatus GetByDeviceId(Guid DeviceId);
        public DeviceStatus GetByMachineName(string MachineName);
        public IList<DeviceStatus> GetAllAsync();
    }

}
