using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Keyless]
    public class ThisDevice
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string DeviceNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string DeviceLocation { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [StringLength(128)]
        public string MachineName { get; set; }
        public Guid BranchId { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string Description { get; set; }
        public int TypeId { get; set; }
        public bool Enabled { get; set; }
        public int ConfigGroup { get; set; }
        public int? UserGroup { get; set; }
        public int GuiscreenList { get; set; }
        public int? LanguageList { get; set; }
        public int CurrencyList { get; set; }
        public int TransactionTypeList { get; set; }
        public int LoginCycles { get; set; }
        public int LoginAttempts { get; set; }
    }
}
