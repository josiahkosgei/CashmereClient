using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class AlertEmailRepository : RepositoryBase<AlertEmail>, IAlertEmailRepository
    {
        public AlertEmailRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<AlertEmail>> GetByAlertEventIdAsync(Guid alertEventId)
        {
            return await DbContext.AlertEmails.Where(x=>x.AlertEventId ==alertEventId).ToListAsync();
        }
    }
}