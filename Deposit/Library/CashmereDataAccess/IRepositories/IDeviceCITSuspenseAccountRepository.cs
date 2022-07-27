using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IDeviceCITSuspenseAccountRepository : IAsyncRepository<DeviceCITSuspenseAccount>
    {
        public DeviceCITSuspenseAccount GetAccountNumber(Guid deviceId, string key);
    }
}