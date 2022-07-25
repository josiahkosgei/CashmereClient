using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IAlertSMSRepository : IAsyncRepository<AlertSMS>
    {
        Task<List<AlertSMS>> GetByAlertEventId(Guid alertEventId);
    }
}