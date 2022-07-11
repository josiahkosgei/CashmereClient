// Type: CashmereDeposit.AlertAttachmentType

using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class AlertAttachmentType
    {

        [Key]
        public Guid Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? AlertTypeId { get; set; }

        public bool Enabled { get; set; }

        public string MimeType { get; set; }

        public string MimeSubtype { get; set; }
    }
}
