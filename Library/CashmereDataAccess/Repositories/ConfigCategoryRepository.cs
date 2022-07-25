using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ConfigCategoryRepository : RepositoryBase<ConfigCategory>, IConfigCategoryRepository
    {
        public ConfigCategoryRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}