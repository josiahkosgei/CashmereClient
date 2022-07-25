using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TextItemRepository : RepositoryBase<TextItem>, ITextItemRepository
    {
        public TextItemRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}