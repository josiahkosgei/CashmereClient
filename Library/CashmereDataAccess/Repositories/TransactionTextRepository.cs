using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionTextRepository : RepositoryBase<TransactionText>, ITransactionTextRepository
    {
        public TransactionTextRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}