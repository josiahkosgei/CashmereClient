using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Keyless]
    public partial class ViewConfig
    {
        [Required]
        [Column("config_id")]
        [StringLength(50)]
        public string ConfigId { get; set; }
        [Required]
        [Column("config_value")]
        public string ConfigValue { get; set; }
    }
}
