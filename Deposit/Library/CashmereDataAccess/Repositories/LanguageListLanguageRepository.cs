using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class LanguageListLanguageRepository : RepositoryBase<LanguageListLanguage>, ILanguageListLanguageRepository
    {
        public LanguageListLanguageRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}