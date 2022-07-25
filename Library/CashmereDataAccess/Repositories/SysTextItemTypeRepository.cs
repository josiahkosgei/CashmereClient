using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class SysTextItemTypeRepository : RepositoryBase<SysTextItemType>, ISysTextItemTypeRepository
    {
        public SysTextItemTypeRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

    }
}