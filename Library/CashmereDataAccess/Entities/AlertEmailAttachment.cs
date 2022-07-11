
// Type: CashmereDeposit.AlertEmailAttachment


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class AlertEmailAttachment
    {
        [Key]
        public Guid Id { get; set; }

        public Guid AlertEmailId { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public string Type { get; set; }

        public byte[] Data { get; set; }

        public byte[] Hash { get; set; }
    }
}
