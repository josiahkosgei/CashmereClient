using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionExceptionRepository : RepositoryBase<TransactionException>, ITransactionExceptionRepository
    {
        public TransactionExceptionRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}