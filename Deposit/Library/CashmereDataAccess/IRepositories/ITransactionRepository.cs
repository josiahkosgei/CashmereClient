using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface ITransactionRepository : IAsyncRepository<Transaction>
    {
        public Transaction GetFirst();
        public Transaction GetFirstSortBy();
        public IList<Transaction> GetCompleted();
        public IList<Transaction> GetByDeviceDateRange(Guid CITId, Guid DeviceId, DateTime FromDate, DateTime ToDate);

        public IQueryable<Transaction> GetByDateRange(DateTime StartDate, DateTime EndDate);
    }

}
