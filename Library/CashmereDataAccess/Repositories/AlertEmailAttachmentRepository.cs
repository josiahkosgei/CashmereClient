using Cashmere.API.Messaging.Communication.Emails;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class AlertEmailAttachmentRepository : RepositoryBase<AlertEmailAttachment>, IAlertEmailAttachmentRepository
    {
        public AlertEmailAttachmentRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public List<EmailAttachment> GetAlertEmailAttachments(Guid alertEmailId)
        {
            var response = DbContext.AlertEmailAttachments.Where(y => y.AlertEmailId == alertEmailId).ToList().Join(DbContext.AlertAttachmentTypes, alertEmailAttachment => alertEmailAttachment.Type, alertAttachmentType => alertAttachmentType.Code, (alertEmailAttachment, alertAttachmentType) => new EmailAttachment()
            {
                Name = alertEmailAttachment.Name,
                MimeType = new EmailAttachmentMIMEType()
                {
                    MimeType = alertAttachmentType.MimeType,
                    MimeSubType = alertAttachmentType.MimeSubtype
                },
                Data = alertEmailAttachment.Data
            }).ToList();
            return response;
        }
    }
}