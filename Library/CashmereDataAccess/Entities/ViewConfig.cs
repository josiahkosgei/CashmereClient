using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Keyless]
    public class ViewConfig
    {
        [Required]
        [StringLength(50)]
        public string ConfigId { get; set; }
        [Required]
        public string ConfigValue { get; set; }
    }
}
