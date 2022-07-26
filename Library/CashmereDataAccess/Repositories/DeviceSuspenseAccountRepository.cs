using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DeviceSuspenseAccountRepository : RepositoryBase<DeviceSuspenseAccount>, IDeviceSuspenseAccountRepository
    {
        public DeviceSuspenseAccountRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<DeviceSuspenseAccount> GetAccountNumber(Guid deviceId, string currencyKey)
        {
            var result = depositorDBContext.DeviceSuspenseAccounts.Where(x => x.DeviceId == deviceId && x.CurrencyCode.Equals(currencyKey, StringComparison.OrdinalIgnoreCase) && x.Enabled == true).FirstOrDefault();
            return await Task.Run<DeviceSuspenseAccount>(() => result);
        }
    }
}