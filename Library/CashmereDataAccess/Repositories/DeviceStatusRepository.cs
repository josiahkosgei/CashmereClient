using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DeviceStatusRepository : RepositoryBase<DeviceStatus>, IDeviceStatusRepository
    {
        public DeviceStatusRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<DeviceStatus> GetByDeviceId(Guid DeviceId)
        {
            var result = depositorDBContext.DeviceStatus.Where(y => y.DeviceId == DeviceId).FirstOrDefault();
            return await Task.Run<DeviceStatus>(() => result);
        }
        public async Task<DeviceStatus> GetByMachineName(string MachineName)
        {
            var result = depositorDBContext.DeviceStatus.Where(y => y.MachineName == MachineName).FirstOrDefault();
            return await Task.Run<DeviceStatus>(() => result);
        }

        public async Task<IList<DeviceStatus>> GetAllAsync()
        {
            var result = depositorDBContext.DeviceStatus.ToList();
            return await Task.Run<IList<DeviceStatus>>(() => result);
        }
    }
}
