using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionLimitListRepository : RepositoryBase<TransactionLimitList>, ITransactionLimitListRepository
    {
        public TransactionLimitListRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}