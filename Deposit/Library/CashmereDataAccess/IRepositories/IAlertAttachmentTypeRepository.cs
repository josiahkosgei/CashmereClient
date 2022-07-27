using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IAlertAttachmentTypeRepository : IAsyncRepository<AlertAttachmentType>
    {
        AlertAttachmentType GetByCode(string code);
    }
}