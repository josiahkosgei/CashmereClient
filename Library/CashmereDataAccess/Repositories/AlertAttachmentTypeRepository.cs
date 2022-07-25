using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class AlertAttachmentTypeRepository : RepositoryBase<AlertAttachmentType>, IAlertAttachmentTypeRepository
    {
        public AlertAttachmentTypeRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<AlertAttachmentType> GetByCode(string code)
        {
           return await DbContext.AlertAttachmentTypes.Where(x => x.Code.Equals("130001", StringComparison.Ordinal)).FirstOrDefaultAsync();
        }
    }
}