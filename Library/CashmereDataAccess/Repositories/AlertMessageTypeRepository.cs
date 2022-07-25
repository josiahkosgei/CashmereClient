using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class AlertMessageTypeRepository : RepositoryBase<AlertMessageType>, IAlertMessageTypeRepository
    {
        public AlertMessageTypeRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public Task<List<AlertMessageType>> GetEnabled()
        {
            return DbContext.AlertMessageTypes.Where(x => x.Enabled.Value == true).ToListAsync();
        }
    }
}