using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DevicePrinterRepository : RepositoryBase<DevicePrinter>, IDevicePrinterRepository
    {
        public DevicePrinterRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}