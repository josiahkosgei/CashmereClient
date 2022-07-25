using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IAlertMessageTypeRepository : IAsyncRepository<AlertMessageType>
    {
       public Task<List<AlertMessageType>> GetEnabled();
    }
}