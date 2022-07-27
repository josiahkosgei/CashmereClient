using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DeviceCITSuspenseAccountRepository : RepositoryBase<DeviceCITSuspenseAccount>, IDeviceCITSuspenseAccountRepository
    {
        public DeviceCITSuspenseAccountRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public  DeviceCITSuspenseAccount GetAccountNumber(Guid deviceId, string currencyKey)
        {
            var result = _depositorDBContext.DeviceCITSuspenseAccounts.Where(x => x.DeviceId == deviceId && x.CurrencyCode.Equals(currencyKey, StringComparison.OrdinalIgnoreCase) && x.Enabled == true).FirstOrDefault();
            return result;
        }
    }
}