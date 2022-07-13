﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("Permission")]
    // [Index("RoleId", "ActivityId", Name = "UX_Permission", IsUnique = true)]
    // [Index("ActivityId", Name = "iactivity_id_Permission")]
    // [Index("RoleId", Name = "irole_id_Permission")]
    public partial class Permission
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("role_id")]
        public Guid RoleId { get; set; }
        [Column("activity_id")]
        public Guid ActivityId { get; set; }
        [Column("standalone_allowed")]
        public bool StandaloneAllowed { get; set; }
        [Column("standalone_authentication_required")]
        public bool StandaloneAuthenticationRequired { get; set; }
        [Column("standalone_can_Authenticate")]
        public bool StandaloneCanAuthenticate { get; set; }

        [ForeignKey("ActivityId")]
        //[InverseProperty("Permissions")]
        public virtual Activity Activity { get; set; } = null!;
        [ForeignKey("RoleId")]
        //[InverseProperty("Permissions")]
        public virtual Role Role { get; set; } = null!;
    }
}