using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class AlertEventRepository : RepositoryBase<AlertEvent>, IAlertEventRepository
    {
        public AlertEventRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<AlertEvent> GetAlertEventAsync(int alertTypeId)
        {
            var result = depositorDBContext.AlertEvents.Where(x => x.AlertTypeId == alertTypeId).OrderByDescending(x => x.Created).FirstOrDefault();
            return await Task.Run<AlertEvent>(() => result);
        }

        public async Task<List<AlertEvent>> GetUnProcessedAsync(int _alertBatchSize)
        {
            var result = depositorDBContext.AlertEvents.Where(x => x.IsProcessed == false).OrderBy(y => y.DateDetected).Take(_alertBatchSize).ToList();
            return await Task.Run<List<AlertEvent>>(() => result);
        }
    }
}