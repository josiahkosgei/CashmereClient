using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TextItemTypeRepository : RepositoryBase<TextItemType>, ITextItemTypeRepository
    {
        public TextItemTypeRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}