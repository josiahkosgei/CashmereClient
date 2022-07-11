using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("Country")]
    public partial class Country
    {
        public Country()
        {
            Banks = new HashSet<Bank>();
        }

        [Key]
        [Column("country_code")]
        [StringLength(2)]
        [Unicode(false)]
        public string CountryCode { get; set; }
        [Required]
        [Column("country_name")]
        [StringLength(100)]
        [Unicode(false)]
        public string CountryName { get; set; }

        [InverseProperty(nameof(Bank.CountryCodeNavigation))]
        public virtual ICollection<Bank> Banks { get; set; }
    }
}
