using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class LanguageListRepository : RepositoryBase<LanguageList>, ILanguageListRepository
    {
        public LanguageListRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}