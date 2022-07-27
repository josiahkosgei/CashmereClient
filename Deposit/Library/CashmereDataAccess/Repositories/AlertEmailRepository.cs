using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class AlertEmailRepository : RepositoryBase<AlertEmail>, IAlertEmailRepository
    {
        public AlertEmailRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public List<AlertEmail> GetByAlertEventIdAsync(Guid alertEventId)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.AlertEmails.Where(x => x.AlertEventId == alertEventId).ToList();
                return result;

            }
        }
    }
}