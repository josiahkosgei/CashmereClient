using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    /// <summary>
    /// Result of sending an alert email
    /// </summary>
    public class AlertEmailResult
    {
        [Key]
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public Guid AlertEmailId { get; set; }
        [Required]
        [StringLength(10)]
        public string Status { get; set; }
        public DateTime? DateSent { get; set; }
        public bool IsSent { get; set; }
        [Required]
        [StringLength(50)]
        public string Recipient { get; set; }
        public int Error { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsProcessed { get; set; }
        public string HtmlMessage { get; set; }
        public string RawTextMessage { get; set; }
    }
}
