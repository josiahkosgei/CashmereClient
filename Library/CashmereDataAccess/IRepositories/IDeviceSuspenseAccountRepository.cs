using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IDeviceSuspenseAccountRepository : IAsyncRepository<DeviceSuspenseAccount>
    {
        public Task<DeviceSuspenseAccount> GetAccountNumber(Guid deviceId, string key);

    }
}