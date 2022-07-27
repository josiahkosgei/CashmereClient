using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface ICITRepository : IAsyncRepository<CIT>
    {

        public CIT GetByDeviceId(Guid Id);
        public IQueryable<CIT> GetByDateRange(DateTime txQueryStartDate, DateTime txQueryEndDate);
        public CIT GetByIdAsync(Guid DeviceId);
        public IList<CIT> GetInCompleteByDeviceId(Guid lastCITId, Guid DeviceId);
        public CIT LastCIT(Guid lastCITId);
        public bool ValidateSealNumberAsync(string newSealNumber);
    }

}
