using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Keyless]
    public partial class ThisDevice
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [Column("device_number")]
        [StringLength(50)]
        public string DeviceNumber { get; set; }
        [Required]
        [Column("device_location")]
        [StringLength(50)]
        public string DeviceLocation { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [Column("machine_name")]
        [StringLength(128)]
        public string MachineName { get; set; }
        [Column("branch_id")]
        public Guid BranchId { get; set; }
        [Column("description")]
        [StringLength(255)]
        [Unicode(false)]
        public string Description { get; set; }
        [Column("type_id")]
        public int TypeId { get; set; }
        [Column("enabled")]
        public bool Enabled { get; set; }
        [Column("config_group")]
        public int ConfigGroup { get; set; }
        [Column("user_group")]
        public int? UserGroup { get; set; }
        [Column("GUIScreen_list")]
        public int GuiscreenList { get; set; }
        [Column("language_list")]
        public int? LanguageList { get; set; }
        [Column("currency_list")]
        public int CurrencyList { get; set; }
        [Column("transaction_type_list")]
        public int TransactionTypeList { get; set; }
        [Column("login_cycles")]
        public int LoginCycles { get; set; }
        [Column("login_attempts")]
        public int LoginAttempts { get; set; }
    }
}
