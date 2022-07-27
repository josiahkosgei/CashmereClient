using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class AlertMessageTypeRepository : RepositoryBase<AlertMessageType>, IAlertMessageTypeRepository
    {
        public AlertMessageTypeRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public List<AlertMessageType> GetEnabled()
        {

            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.AlertMessageTypes.Where(x => x.Enabled.Value == true).ToList();
                return result;

            }
        }
    }
}