using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ConfigGroupRepository : RepositoryBase<ConfigGroup>, IConfigGroupRepository
    {
        public ConfigGroupRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}