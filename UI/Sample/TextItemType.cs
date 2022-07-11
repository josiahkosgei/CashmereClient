using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("TextItemType", Schema = "xlns")]
    [Index(nameof(Token), Name = "UX_UI_TextItemType", IsUnique = true)]
    public partial class TextItemType
    {
        public TextItemType()
        {
            TextItems = new HashSet<TextItem>();
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

        [InverseProperty(nameof(TextItem.TextItemType))]
        public virtual ICollection<TextItem> TextItems { get; set; }
    }
}
