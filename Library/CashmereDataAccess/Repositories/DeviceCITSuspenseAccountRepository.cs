using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DeviceCITSuspenseAccountRepository : RepositoryBase<DeviceCITSuspenseAccount>, IDeviceCITSuspenseAccountRepository
    {
        public DeviceCITSuspenseAccountRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<DeviceCITSuspenseAccount> GetAccountNumber(Guid deviceId, string currencyKey)
        {
            return await DbContext.DeviceCITSuspenseAccounts.Where(x => x.DeviceId == deviceId && x.CurrencyCode.Equals(currencyKey, StringComparison.OrdinalIgnoreCase) && x.Enabled == true).FirstOrDefaultAsync();
        }
    }
}