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

        public async Task<AlertAttachmentType> GetByCode(string code)
        {
            var result = depositorDBContext.AlertAttachmentTypes.Where(x => x.Code.Equals("130001", StringComparison.Ordinal)).FirstOrDefault();
            return await Task.Run<AlertAttachmentType>(() => result);
        }
    }
}