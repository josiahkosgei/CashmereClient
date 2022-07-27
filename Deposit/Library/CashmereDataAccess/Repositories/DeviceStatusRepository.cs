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

        public  DeviceStatus GetByDeviceId(Guid DeviceId)
        {
            var result = _depositorDBContext.DeviceStatus.Where(y => y.DeviceId == DeviceId).FirstOrDefault();
            return result;
        }
        public  DeviceStatus GetByMachineName(string MachineName)
        {
            var result = _depositorDBContext.DeviceStatus.Where(y => y.MachineName == MachineName).FirstOrDefault();
            return result;
        }

        public  IList<DeviceStatus> GetAllAsync()
        {
            var result = _depositorDBContext.DeviceStatus.ToList();
            return result;
        }
    }
}
