﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample.Entities
{
    [Keyless]
    public partial class ThisDevice
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("device_number")]
        [StringLength(50)]
        public string DeviceNumber { get; set; } = null!;
        [Column("device_location")]
        [StringLength(50)]
        public string DeviceLocation { get; set; } = null!;
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [Column("machine_name")]
        [StringLength(128)]
        public string? MachineName { get; set; }
        [Column("branch_id")]
        public Guid BranchId { get; set; }
        [Column("description")]
        [StringLength(255)]
        public string? Description { get; set; }
        [Column("type_id")]
        public int TypeId { get; set; }
        [Column("enabled")]
        public bool Enabled { get; set; }
        [Column("config_group")]
        public int ConfigGroup { get; set; }
        [Column("user_group")]
        public int? UserGroup { get; set; }
        [Column("GUIScreen_list")]
        public int GUIScreenList { get; set; }
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