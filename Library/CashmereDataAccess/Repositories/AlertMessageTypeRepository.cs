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

        public async Task<List<AlertMessageType>> GetEnabled()
        {
            var result = depositorDBContext.AlertMessageTypes.Where(x => x.Enabled.Value == true).ToList();
            return await Task.Run<List<AlertMessageType>>(() => result);
        }
    }
}