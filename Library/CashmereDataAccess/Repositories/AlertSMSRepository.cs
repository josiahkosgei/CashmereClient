using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class AlertSMSRepository : RepositoryBase<AlertSMS>, IAlertSMSRepository
    {
        public AlertSMSRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<AlertSMS>> GetByAlertEventId(Guid alertEventId)
        {
            return await DbContext.AlertSMS.Where(x => x.AlertEventId == alertEventId).ToListAsync();
        }
    }
}