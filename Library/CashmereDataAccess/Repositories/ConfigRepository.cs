using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ConfigRepository : RepositoryBase<Config>, IConfigRepository
    {
        public ConfigRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}