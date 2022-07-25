using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface ICITRepository : IAsyncRepository<CIT>
    {

        public Task<CIT> GetByDeviceId(Guid Id);
        public IQueryable<CIT> GetByDateRange(DateTime txQueryStartDate, DateTime txQueryEndDate);
        public Task<CIT> GetByIdAsync(Guid DeviceId);
        public Task<IList<CIT>> GetInCompleteByDeviceId(Guid lastCITId, Guid DeviceId);
        public Task<CIT> LastCIT(Guid lastCITId);
        public Task<bool> ValidateSealNumberAsync(string newSealNumber);
    }

}
