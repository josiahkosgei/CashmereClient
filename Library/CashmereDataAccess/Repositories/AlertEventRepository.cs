using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class AlertEventRepository : RepositoryBase<AlertEvent>, IAlertEventRepository
    {
        public AlertEventRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<AlertEvent> GetAlertEventAsync(int alertTypeId)
        {
            return await DbContext.AlertEvents.Where(x => x.AlertTypeId == alertTypeId).OrderByDescending(x => x.Created).FirstOrDefaultAsync();
        }

        public async Task<List<AlertEvent>> GetUnProcessedAsync(int _alertBatchSize)
        {
            return await DbContext.AlertEvents.Where(x => x.IsProcessed == false).OrderBy(y => y.DateDetected).Take(_alertBatchSize).ToListAsync();
        }
    }
}