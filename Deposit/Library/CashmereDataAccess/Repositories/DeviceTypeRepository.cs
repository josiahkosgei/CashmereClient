using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DeviceTypeRepository : RepositoryBase<DeviceType>, IDeviceTypeRepository
    {
        public DeviceTypeRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}