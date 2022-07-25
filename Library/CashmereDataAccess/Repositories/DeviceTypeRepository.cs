using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DeviceTypeRepository : RepositoryBase<DeviceType>, IDeviceTypeRepository
    {
        public DeviceTypeRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}