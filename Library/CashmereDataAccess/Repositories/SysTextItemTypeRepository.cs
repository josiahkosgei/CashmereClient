using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class SysTextItemTypeRepository : RepositoryBase<SysTextItemType>, ISysTextItemTypeRepository
    {
        public SysTextItemTypeRepository(IConfiguration configuration) : base(configuration)
        {
        }

    }
}