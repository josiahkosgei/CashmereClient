using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("GUIScreenType")]
    public partial class GuiscreenType
    {
        public GuiscreenType()
        {
            Guiscreens = new HashSet<Guiscreen>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("code")]
        public Guid Code { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [Column("description")]
        [StringLength(255)]
        [Unicode(false)]
        public string Description { get; set; }
        [Column("enabled")]
        public bool Enabled { get; set; }

        [InverseProperty(nameof(Guiscreen.TypeNavigation))]
        public virtual ICollection<Guiscreen> Guiscreens { get; set; }
    }
}
