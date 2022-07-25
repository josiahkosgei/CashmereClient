using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionTypeListTransactionTypeListItemRepository : RepositoryBase<TransactionTypeListTransactionTypeListItem>, ITransactionTypeListTransactionTypeListItemRepository
    {
        public TransactionTypeListTransactionTypeListItemRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}