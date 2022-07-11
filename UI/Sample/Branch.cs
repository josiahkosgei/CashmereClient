using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("Branch")]
    [Index(nameof(BankId), Name = "ibank_id_Branch")]
    public partial class Branch
    {
        public Branch()
        {
            Devices = new HashSet<Device>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [Column("branch_code")]
        [StringLength(50)]
        public string BranchCode { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("description")]
        [StringLength(255)]
        public string Description { get; set; }
        [Column("bank_id")]
        public Guid BankId { get; set; }

        [ForeignKey(nameof(BankId))]
        [InverseProperty("Branches")]
        public virtual Bank Bank { get; set; }
        [InverseProperty(nameof(Device.Branch))]
        public virtual ICollection<Device> Devices { get; set; }
    }
}
