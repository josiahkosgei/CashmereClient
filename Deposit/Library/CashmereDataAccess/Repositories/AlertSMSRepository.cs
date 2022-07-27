using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class AlertSMSRepository : RepositoryBase<AlertSMS>, IAlertSMSRepository
    {
        public AlertSMSRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public List<AlertSMS> GetByAlertEventId(Guid alertEventId)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.AlertSMS.Where(x => x.AlertEventId == alertEventId).ToList();
                return result;

            }
        }
    }
}