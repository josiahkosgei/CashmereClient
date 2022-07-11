using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// The bank that owns the depositor
    /// </summary>
    [Table("Bank")]
    [Index(nameof(CountryCode), Name = "icountry_code_Bank")]
    public partial class Bank
    {
        public Bank()
        {
            Branches = new HashSet<Branch>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [Column("bank_code")]
        [StringLength(50)]
        public string BankCode { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [Column("description")]
        [StringLength(255)]
        public string Description { get; set; }
        [Required]
        [Column("country_code")]
        [StringLength(2)]
        [Unicode(false)]
        public string CountryCode { get; set; }

        [ForeignKey(nameof(CountryCode))]
        [InverseProperty(nameof(Country.Banks))]
        public virtual Country CountryCodeNavigation { get; set; }
        [InverseProperty(nameof(Branch.Bank))]
        public virtual ICollection<Branch> Branches { get; set; }
    }
}
