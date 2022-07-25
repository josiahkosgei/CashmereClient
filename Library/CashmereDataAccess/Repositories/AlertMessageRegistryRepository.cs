using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class AlertMessageRegistryRepository : RepositoryBase<AlertMessageRegistry>, IAlertMessageRegistryRepository
    {
        public AlertMessageRegistryRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}