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

        public  List<AlertEmail> GetByAlertEventIdAsync(Guid alertEventId)
        {
            var result = _depositorDBContext.AlertEmails.Where(x => x.AlertEventId == alertEventId).ToList();
            return result;
        }
    }
}