using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class LanguageListRepository : RepositoryBase<LanguageList>, ILanguageListRepository
    {
        public LanguageListRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}