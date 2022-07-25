using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IAlertEmailRepository : IAsyncRepository<AlertEmail>
    {
        Task<List<AlertEmail>> GetByAlertEventIdAsync(Guid alertEventId);
    }
}