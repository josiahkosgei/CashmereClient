using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ConfigCategoryRepository : RepositoryBase<ConfigCategory>, IConfigCategoryRepository
    {
        public ConfigCategoryRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}