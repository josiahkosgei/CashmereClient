using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class SysTextItemCategoryRepository : RepositoryBase<SysTextItemCategory>, ISysTextItemCategoryRepository
    {
        public SysTextItemCategoryRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }        
    }
}