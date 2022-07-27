using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("sysTextItemCategory", Schema = "xlns")]
    public partial class SysTextItemCategory
    {
        public SysTextItemCategory()
        {
            InverseParentNavigation = new HashSet<SysTextItemCategory>();
            SysTextItems = new HashSet<SysTextItem>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [Column("description")]
        [StringLength(255)]
        public string? Description { get; set; }
        public Guid? Parent { get; set; }

        [ForeignKey("Parent")]
        public virtual SysTextItemCategory? ParentNavigation { get; set; }
        public virtual ICollection<SysTextItemCategory> InverseParentNavigation { get; set; }
        public virtual ICollection<SysTextItem> SysTextItems { get; set; }
    }
}