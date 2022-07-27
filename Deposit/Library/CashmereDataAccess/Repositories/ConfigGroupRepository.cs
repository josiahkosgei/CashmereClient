using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ConfigGroupRepository : RepositoryBase<ConfigGroup>, IConfigGroupRepository
    {
        public ConfigGroupRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}