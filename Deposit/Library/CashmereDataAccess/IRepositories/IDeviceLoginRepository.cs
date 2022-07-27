using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IDeviceLoginRepository : IAsyncRepository<DeviceLogin>
    {
        public DeviceLogin GetFirst(Guid Id);
    }
}
