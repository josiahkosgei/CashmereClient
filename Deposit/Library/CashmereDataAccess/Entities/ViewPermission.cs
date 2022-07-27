using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Keyless]
    public class ViewPermission
    {
        [Required]
        [StringLength(50)]
        public string Role { get; set; }
        [Required]
        [StringLength(50)]
        public string Activity { get; set; }
        public bool StandaloneAllowed { get; set; }
        public bool StandaloneAuthenticationRequired { get; set; }
        public bool StandaloneCanAuthenticate { get; set; }
    }
}
