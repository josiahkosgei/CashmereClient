using Cashmere.API.Messaging.Communication.Emails;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class AlertEmailAttachmentRepository : RepositoryBase<AlertEmailAttachment>, IAlertEmailAttachmentRepository
    {
        public AlertEmailAttachmentRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public List<EmailAttachment> GetAlertEmailAttachments(Guid alertEmailId)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var response = dbContext.AlertEmailAttachments.Where(y => y.AlertEmailId == alertEmailId).ToList().Join(dbContext.AlertAttachmentTypes, alertEmailAttachment => alertEmailAttachment.Type, alertAttachmentType => alertAttachmentType.Code, (alertEmailAttachment, alertAttachmentType) => new EmailAttachment()
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
}