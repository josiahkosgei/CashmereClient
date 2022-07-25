using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionLimitListItemRepository : RepositoryBase<TransactionLimitListItem>, ITransactionLimitListItemRepository
    {
        public TransactionLimitListItemRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}