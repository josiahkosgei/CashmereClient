using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class SysTextItemRepository : RepositoryBase<SysTextItem>, ISysTextItemRepository
    {
        public SysTextItemRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
        public async Task<SysTextItem> GetByTokenId(string tokenID)
        {
            return await DbContext.SysTextItems.Where(x => x.Token == tokenID).FirstOrDefaultAsync();
        }
    }
}