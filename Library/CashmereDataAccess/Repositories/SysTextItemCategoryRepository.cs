using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class SysTextItemCategoryRepository : RepositoryBase<SysTextItemCategory>, ISysTextItemCategoryRepository
    {
        public SysTextItemCategoryRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}