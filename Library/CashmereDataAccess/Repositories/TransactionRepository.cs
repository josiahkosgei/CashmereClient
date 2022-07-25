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

        public IQueryable<Transaction> GetByDateRange(DateTime txQueryStartDate, DateTime txQueryEndDate)
        {
            return DbContext.Transactions.Where(t => t.TxEndDate >= txQueryStartDate && t.TxEndDate < txQueryEndDate).OrderByDescending(t => t.TxEndDate).AsQueryable();
        }

        public async Task<IList<Transaction>> GetByDeviceDateRange(Guid CITId, Guid DeviceId, DateTime FromDate, DateTime ToDate)
        {
            return await DbContext.Transactions.Where(x => x.CITId == CITId && x.DeviceId == DeviceId && x.TxStartDate >= FromDate && x.TxStartDate <= ToDate).ToListAsync();
        }

        public async Task<IList<Transaction>> GetCompleted()
        {
            return await DbContext.Transactions.Where(x => x.TxCompleted).ToListAsync();
        }

        public async Task<Transaction> GetFirst()
        {
            return await DbContext.Transactions.OrderByDescending(x => x.CbDate).FirstOrDefaultAsync();
        }
        public async Task<Transaction> GetFirstSortBy()
        {
            return await DbContext.Transactions.OrderByDescending(x => x.TxStartDate).FirstOrDefaultAsync();
        }
    }
}
