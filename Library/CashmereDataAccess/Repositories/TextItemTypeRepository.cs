using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TextItemTypeRepository : RepositoryBase<TextItemType>, ITextItemTypeRepository
    {
        public TextItemTypeRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}