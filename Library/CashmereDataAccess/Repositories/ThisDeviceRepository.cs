using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ThisDeviceRepository : RepositoryBase<ThisDevice>, IThisDeviceRepository
    {
        public ThisDeviceRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}