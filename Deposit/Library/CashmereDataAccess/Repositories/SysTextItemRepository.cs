using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class SysTextItemRepository : RepositoryBase<SysTextItem>, ISysTextItemRepository
    {
        public SysTextItemRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public  SysTextItem GetByTokenId(string tokenID)
        {
            var result = _depositorDBContext.SysTextItems.Where(x => x.Token == tokenID).FirstOrDefault();
            return result;
        }
    }
}