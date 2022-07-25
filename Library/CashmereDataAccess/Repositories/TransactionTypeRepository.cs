using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionTypeRepository : RepositoryBase<TransactionType>, ITransactionTypeRepository
    {
        public TransactionTypeRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}