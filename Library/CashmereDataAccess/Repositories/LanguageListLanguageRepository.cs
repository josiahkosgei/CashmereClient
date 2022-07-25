using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class LanguageListLanguageRepository : RepositoryBase<LanguageListLanguage>, ILanguageListLanguageRepository
    {
        public LanguageListLanguageRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}