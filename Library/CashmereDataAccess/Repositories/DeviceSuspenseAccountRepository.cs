using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DeviceSuspenseAccountRepository : RepositoryBase<DeviceSuspenseAccount>, IDeviceSuspenseAccountRepository
    {
        public DeviceSuspenseAccountRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
         public async Task<DeviceSuspenseAccount> GetAccountNumber(Guid deviceId, string currencyKey)
        {
            return await DbContext.DeviceSuspenseAccounts.Where(x => x.DeviceId == deviceId && x.CurrencyCode.Equals(currencyKey, StringComparison.OrdinalIgnoreCase) && x.Enabled == true).FirstOrDefaultAsync();
        }
    }
}