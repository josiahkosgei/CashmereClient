using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ConfigRepository : RepositoryBase<Config>, IConfigRepository
    {
        public ConfigRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}