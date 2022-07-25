using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionTypeListRepository : RepositoryBase<TransactionTypeList>, ITransactionTypeListRepository
    {
        public TransactionTypeListRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}