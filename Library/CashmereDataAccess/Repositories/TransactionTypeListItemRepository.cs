using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionTypeListItemRepository : RepositoryBase<TransactionTypeListItem>, ITransactionTypeListItemRepository
    {
        public TransactionTypeListItemRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

    }
}
