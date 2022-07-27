using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TextItemCategoryRepository : RepositoryBase<TextItemCategory>, ITextItemCategoryRepository
    {
        public TextItemCategoryRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}