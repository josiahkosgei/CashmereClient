using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IAlertAttachmentTypeRepository : IAsyncRepository<AlertAttachmentType>
    {
        Task<AlertAttachmentType> GetByCode(string code);
    }
}