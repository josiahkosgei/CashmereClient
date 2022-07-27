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
            return _depositorDBContext.Transactions.Where(t => t.TxEndDate >= txQueryStartDate && t.TxEndDate < txQueryEndDate).OrderByDescending(t => t.TxEndDate).AsQueryable();
        }

        public  IList<Transaction> GetByDeviceDateRange(Guid CITId, Guid DeviceId, DateTime FromDate, DateTime ToDate)
        {
            var result = _depositorDBContext.Transactions.Where(x => x.CITId == CITId && x.DeviceId == DeviceId && x.TxStartDate >= FromDate && x.TxStartDate <= ToDate).ToList();
            return result;
        }

        public  IList<Transaction> GetCompleted()
        {
            var result = _depositorDBContext.Transactions.Where(x => x.TxCompleted).ToList();
            return result;
        }

        public  Transaction GetFirst()
        {
            var result = _depositorDBContext.Transactions.OrderByDescending(x => x.CbDate).FirstOrDefault();
            return result;
        }
        public  Transaction GetFirstSortBy()
        {
            var result = _depositorDBContext.Transactions.OrderByDescending(x => x.TxStartDate).FirstOrDefault();
            return result;
        }
    }
}
