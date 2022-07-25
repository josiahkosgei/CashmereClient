using Cashmere.API.Messaging.Communication.Emails;
using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IAlertEmailAttachmentRepository : IAsyncRepository<AlertEmailAttachment>
    {
        public List<EmailAttachment> GetAlertEmailAttachments(Guid alertEmailId);
    }
}