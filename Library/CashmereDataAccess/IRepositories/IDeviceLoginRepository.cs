using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IDeviceLoginRepository : IAsyncRepository<DeviceLogin>
    {
        public Task<DeviceLogin> GetFirst(Guid Id);
    }
}
