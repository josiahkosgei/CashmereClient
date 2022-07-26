using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface ITransactionRepository : IAsyncRepository<Transaction>
    {
        public Task<Transaction> GetFirst();
        public Task<Transaction> GetFirstSortBy();
        public Task<IList<Transaction>> GetCompleted();
        public Task<IList<Transaction>> GetByDeviceDateRange(Guid CITId, Guid DeviceId, DateTime FromDate, DateTime ToDate);

        public IQueryable<Transaction> GetByDateRange(DateTime StartDate, DateTime EndDate);
    }

}
