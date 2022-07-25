using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IAlertEventRepository : IAsyncRepository<AlertEvent>
    {
        public Task<AlertEvent> GetAlertEventAsync(int alertTypeId);
        public Task<List<AlertEvent>> GetUnProcessedAsync(int _alertBatchSize);
    }
}