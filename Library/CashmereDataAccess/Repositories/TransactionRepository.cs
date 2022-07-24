using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<IList<Transaction>> GetCompleted()
        {
            return await DbContext.Transactions.Where(x => x.TxCompleted).ToListAsync();
        }

        public async Task<Transaction> GetFirst()
        {
            return await DbContext.Transactions.OrderByDescending(x => x.CbDate).FirstOrDefaultAsync();
        }
    }
}
