using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IAlertEventRepository : IAsyncRepository<AlertEvent>
    {
        public AlertEvent GetAlertEventAsync(int alertTypeId);
        public List<AlertEvent> GetUnProcessedAsync(int _alertBatchSize);
    }
}