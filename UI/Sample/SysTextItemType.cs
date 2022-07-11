using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("sysTextItemType", Schema = "xlns")]
    public partial class SysTextItemType
    {
        public SysTextItemType()
        {
            SysTextItems = new HashSet<SysTextItem>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [Column("token")]
        [StringLength(100)]
        public string Token { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("description")]
        [StringLength(255)]
        public string Description { get; set; }

        [InverseProperty(nameof(SysTextItem.TextItemType))]
        public virtual ICollection<SysTextItem> SysTextItems { get; set; }
    }
}
