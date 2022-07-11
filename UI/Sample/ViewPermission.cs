using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Keyless]
    public partial class ViewPermission
    {
        [Required]
        [Column("role")]
        [StringLength(50)]
        public string Role { get; set; }
        [Required]
        [Column("activity")]
        [StringLength(50)]
        public string Activity { get; set; }
        [Column("standalone_allowed")]
        public bool StandaloneAllowed { get; set; }
        [Column("standalone_authentication_required")]
        public bool StandaloneAuthenticationRequired { get; set; }
        [Column("standalone_can_Authenticate")]
        public bool StandaloneCanAuthenticate { get; set; }
    }
}
