using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class AlertAttachmentTypeRepository : RepositoryBase<AlertAttachmentType>, IAlertAttachmentTypeRepository
    {
        public AlertAttachmentTypeRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public  AlertAttachmentType GetByCode(string code)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
            var result = dbContext.AlertAttachmentTypes.Where(x => x.Code.Equals("130001", StringComparison.Ordinal)).FirstOrDefault();
            return result;

            }
        }
    }
}