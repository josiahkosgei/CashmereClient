using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Describes the type of device
    /// </summary>
    [Table("DeviceType")]
    public partial class DeviceType
    {
        public DeviceType()
        {
            Devices = new HashSet<Device>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [Required]
        [Column("description")]
        [StringLength(255)]
        [Unicode(false)]
        public string Description { get; set; }
        [Column("note_in")]
        public bool NoteIn { get; set; }
        [Column("note_out")]
        public bool NoteOut { get; set; }
        [Column("note_escrow")]
        public bool NoteEscrow { get; set; }
        [Column("coin_in")]
        public bool CoinIn { get; set; }
        [Column("coin_out")]
        public bool CoinOut { get; set; }
        [Column("coin_escrow")]
        public bool CoinEscrow { get; set; }

        [InverseProperty(nameof(Device.Type))]
        public virtual ICollection<Device> Devices { get; set; }
    }
}
