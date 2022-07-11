using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("ApplicationUser")]
    [Index(nameof(Username), Name = "UX_ApplicationUser_Username", IsUnique = true)]
    [Index(nameof(ApplicationUserLoginDetail), Name = "iApplicationUserLoginDetail_ApplicationUser")]
    [Index(nameof(RoleId), Name = "irole_id_ApplicationUser")]
    [Index(nameof(UserGroup), Name = "iuser_group_ApplicationUser")]
    public partial class ApplicationUser
    {
        public ApplicationUser()
        {
            ApplicationUserChangePasswords = new HashSet<ApplicationUserChangePassword>();
            ApplicationUserLoginDetails = new HashSet<ApplicationUserLoginDetail>();
            CITAuthUserNavigations = new HashSet<CIT>();
            CITStartUserNavigations = new HashSet<CIT>();
            DeviceLocks = new HashSet<DeviceLock>();
            DeviceLogins = new HashSet<DeviceLogin>();
            EscrowJamAuthorisingUserNavigations = new HashSet<EscrowJam>();
            EscrowJamInitialisingUserNavigations = new HashSet<EscrowJam>();
            PasswordHistories = new HashSet<PasswordHistory>();
            UserLocks = new HashSet<UserLock>();
            WebPortalRoleRolesApplicationUserApplicationUsers = new HashSet<WebPortalRoleRolesApplicationUserApplicationUser>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// username for logging into the system
        /// </summary>
        [Required]
        [Column("username")]
        [StringLength(255)]
        public string Username { get; set; }

        /// <summary>
        /// salted and hashed password utilising a password library
        /// </summary>
        [Required]
        [Column("password")]
        [StringLength(71)]
        [Unicode(false)]
        public string Password { get; set; }

        /// <summary>
        /// First names
        /// </summary>
        [Required]
        [Column("fname")]
        [StringLength(50)]
        public string Fname { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        [Required]
        [Column("lname")]
        [StringLength(50)]
        public string Lname { get; set; }

        /// <summary>
        /// The role the user has e.g. Custodian, Branch Manager tc
        /// </summary>
        [Column("role_id")]
        public Guid RoleId { get; set; }

        /// <summary>
        /// user email address, used to receive emails from the system
        /// </summary>
        [Column("email")]
        [StringLength(50)]
        public string Email { get; set; }

        /// <summary>
        /// whether or not the user is allowed to receive emails
        /// </summary>
        [Required]
        [Column("email_enabled")]
        public bool? EmailEnabled { get; set; }

        /// <summary>
        /// the phone number for the user to rceive SMSes from the system
        /// </summary>
        [Column("phone")]
        [StringLength(50)]
        public string Phone { get; set; }

        /// <summary>
        /// can the user receive SMSes from the system
        /// </summary>
        [Column("phone_enabled")]
        public bool PhoneEnabled { get; set; }

        /// <summary>
        /// should the user rset their password at their next login
        /// </summary>
        [Column("password_reset_required")]
        public bool PasswordResetRequired { get; set; }

        /// <summary>
        /// how many unsuccessful login attempts has the user mad in a row. used to lock the user automatically
        /// </summary>
        [Column("login_attempts")]
        public int LoginAttempts { get; set; }
        [Column("user_group")]
        public int UserGroup { get; set; }
        [Column("depositor_enabled")]
        public bool? DepositorEnabled { get; set; }
        public bool? UserDeleted { get; set; }
        public bool? IsActive { get; set; }
        public Guid? ApplicationUserLoginDetail { get; set; }

        [ForeignKey(nameof(ApplicationUserLoginDetail))]
        [InverseProperty("ApplicationUsers")]
        public virtual ApplicationUserLoginDetail ApplicationUserLoginDetailNavigation { get; set; }
        [ForeignKey(nameof(RoleId))]
        [InverseProperty("ApplicationUsers")]
        public virtual Role Role { get; set; }
        [ForeignKey(nameof(UserGroup))]
        [InverseProperty("ApplicationUsers")]
        public virtual UserGroup UserGroupNavigation { get; set; }
        [InverseProperty(nameof(ApplicationUserChangePassword.UserNavigation))]
        public virtual ICollection<ApplicationUserChangePassword> ApplicationUserChangePasswords { get; set; }
        [InverseProperty("UserNavigation")]
        public virtual ICollection<ApplicationUserLoginDetail> ApplicationUserLoginDetails { get; set; }
        [InverseProperty(nameof(CIT.AuthUserNavigation))]
        public virtual ICollection<CIT> CITAuthUserNavigations { get; set; }
        [InverseProperty(nameof(CIT.StartUserNavigation))]
        public virtual ICollection<CIT> CITStartUserNavigations { get; set; }
        [InverseProperty(nameof(DeviceLock.LockingUserNavigation))]
        public virtual ICollection<DeviceLock> DeviceLocks { get; set; }
        [InverseProperty(nameof(DeviceLogin.UserNavigation))]
        public virtual ICollection<DeviceLogin> DeviceLogins { get; set; }
        [InverseProperty(nameof(EscrowJam.AuthorisingUserNavigation))]
        public virtual ICollection<EscrowJam> EscrowJamAuthorisingUserNavigations { get; set; }
        [InverseProperty(nameof(EscrowJam.InitialisingUserNavigation))]
        public virtual ICollection<EscrowJam> EscrowJamInitialisingUserNavigations { get; set; }
        [InverseProperty(nameof(PasswordHistory.UserNavigation))]
        public virtual ICollection<PasswordHistory> PasswordHistories { get; set; }
        [InverseProperty(nameof(UserLock.InitiatingUserNavigation))]
        public virtual ICollection<UserLock> UserLocks { get; set; }
        [InverseProperty(nameof(WebPortalRoleRolesApplicationUserApplicationUser.ApplicationUsersNavigation))]
        public virtual ICollection<WebPortalRoleRolesApplicationUserApplicationUser> WebPortalRoleRolesApplicationUserApplicationUsers { get; set; }
    }
}
