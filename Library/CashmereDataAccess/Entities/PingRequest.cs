using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("PingRequest", Schema = "cb")]
    public class PingRequest
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime MessageDateTime { get; set; }
        [Required]
        [Column("RequestUUID")]
        [StringLength(50)]
        public string RequestUuid { get; set; }
        public bool Success { get; set; }
        public bool ServerOnline { get; set; }
        [Required]
        [StringLength(50)]
        public string Status { get; set; }
        public bool IsError { get; set; }
    }
}
