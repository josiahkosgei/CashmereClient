using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface ICITRepository : IAsyncRepository<CIT>
    {

        public Task<CIT> GetByDeviceId(Guid Id);
        public Task<CIT> GetByIdAsync(Guid DeviceId);
        public Task<IList<CIT>> GetInCompleteByDeviceId(Guid lastCITId,Guid DeviceId);
        public Task<CIT> LastCIT(Guid lastCITId);
    }

}
