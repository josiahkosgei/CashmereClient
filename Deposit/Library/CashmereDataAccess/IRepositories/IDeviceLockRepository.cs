using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IDeviceLockRepository : IAsyncRepository<DeviceLock>
    {
        public DeviceLock GetFirst();
    }

}
