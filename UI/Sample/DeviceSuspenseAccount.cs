using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("DeviceSuspenseAccount")]
    [Index(nameof(DeviceId), nameof(CurrencyCode), Name = "UX_Device_SuspenseAccount", IsUnique = true)]
    [Index(nameof(CurrencyCode), Name = "icurrency_code_DeviceSuspenseAccount")]
    [Index(nameof(DeviceId), Name = "idevice_id_DeviceSuspenseAccount")]
    public partial class DeviceSuspenseAccount
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("device_id")]
        public Guid DeviceId { get; set; }
        [Required]
        [Column("currency_code")]
        [StringLength(3)]
        [Unicode(false)]
        public string CurrencyCode { get; set; }
        [Required]
        [Column("account_number")]
        [StringLength(50)]
        public string AccountNumber { get; set; }
        [Column("account_name")]
        [StringLength(50)]
        public string AccountName { get; set; }
        [Column("enabled")]
        public bool Enabled { get; set; }

        [ForeignKey(nameof(CurrencyCode))]
        [InverseProperty(nameof(Currency.DeviceSuspenseAccounts))]
        public virtual Currency CurrencyCodeNavigation { get; set; }
        [ForeignKey(nameof(DeviceId))]
        [InverseProperty("DeviceSuspenseAccounts")]
        public virtual Device Device { get; set; }
    }
}
