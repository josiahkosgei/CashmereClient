﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Setup.Models
{
    /// <summary>
    /// a user&apos;s role storing all their permissions
    /// </summary>
    [Table("Role")]
    public partial class Role
    {
        public Role()
        {
            AlertMessageRegistries = new HashSet<AlertMessageRegistry>();
            ApplicationUsers = new HashSet<ApplicationUser>();
            Permissions = new HashSet<Permission>();
        }

        [Key]
        [Column("code")]
        public Guid Code { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("description")]
        [StringLength(512)]
        public string Description { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<AlertMessageRegistry> AlertMessageRegistries { get; set; }
        [InverseProperty("Role")]
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        [InverseProperty("Role")]
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}