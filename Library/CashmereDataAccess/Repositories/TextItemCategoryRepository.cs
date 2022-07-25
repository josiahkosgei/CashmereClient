using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TextItemCategoryRepository : RepositoryBase<TextItemCategory>, ITextItemCategoryRepository
    {
        public TextItemCategoryRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}