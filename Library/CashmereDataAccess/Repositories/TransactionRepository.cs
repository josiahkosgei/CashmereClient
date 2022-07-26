using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public IQueryable<Transaction> GetByDateRange(DateTime txQueryStartDate, DateTime txQueryEndDate)
        {
            return depositorDBContext.Transactions.Where(t => t.TxEndDate >= txQueryStartDate && t.TxEndDate < txQueryEndDate).OrderByDescending(t => t.TxEndDate).AsQueryable();
        }

        public async Task<IList<Transaction>> GetByDeviceDateRange(Guid CITId, Guid DeviceId, DateTime FromDate, DateTime ToDate)
        {
            var result = depositorDBContext.Transactions.Where(x => x.CITId == CITId && x.DeviceId == DeviceId && x.TxStartDate >= FromDate && x.TxStartDate <= ToDate).ToList();
            return await Task.Run<IList<Transaction>>(() => result);
        }

        public async Task<IList<Transaction>> GetCompleted()
        {
            var result = depositorDBContext.Transactions.Where(x => x.TxCompleted).ToList();
            return await Task.Run<IList<Transaction>>(() => result);
        }

        public async Task<Transaction> GetFirst()
        {
            var result = depositorDBContext.Transactions.OrderByDescending(x => x.CbDate).FirstOrDefault();
            return await Task.Run<Transaction>(() => result);
        }
        public async Task<Transaction> GetFirstSortBy()
        {
            var result = depositorDBContext.Transactions.OrderByDescending(x => x.TxStartDate).FirstOrDefault();
            return await Task.Run<Transaction>(() => result);
        }
    }
}
