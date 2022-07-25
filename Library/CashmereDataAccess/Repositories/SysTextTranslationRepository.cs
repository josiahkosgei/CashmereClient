using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class SysTextTranslationRepository : RepositoryBase<SysTextTranslation>, ISysTextTranslationRepository
    {
        public SysTextTranslationRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}