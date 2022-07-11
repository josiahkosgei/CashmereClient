
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{

    public  class ApplicationUser
    {
        public ApplicationUser()
        {
            ApplicationUserChangePasswords = new HashSet<ApplicationUserChangePassword>();
            ApplicationUserLoginDetails = new HashSet<ApplicationUserLoginDetail>();
            CITAuthUsers = new HashSet<CIT>();
            CITStartUsers = new HashSet<CIT>();
            DeviceLocks = new HashSet<DeviceLock>();
            DeviceLogins = new HashSet<DeviceLogin>();
            EscrowJamAuthorisingUsers = new HashSet<EscrowJam>();
            EscrowJamInitialisingUsers = new HashSet<EscrowJam>();
            PasswordHistories = new HashSet<PasswordHistory>();
            UserLocks = new HashSet<UserLock>();
            WebPortalRoleRolesApplicationUserApplicationUsers = new HashSet<WebPortalRoleRolesApplicationUserApplicationUser>();
            TransactionPostingAuthUsers = new HashSet<TransactionPosting>();
            TransactionPostingInitUsers = new HashSet<TransactionPosting>();
        }

        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// username for logging into the system
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        /// <summary>
        /// salted and hashed password utilising a password library
        /// </summary>
        [Required]
        [StringLength(71)]
        [Unicode(false)]
        public string Password { get; set; }

        /// <summary>
        /// First names
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Fname { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Lname { get; set; }

        /// <summary>
        /// The role the user has e.g. Custodian, Branch Manager tc
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// user email address, used to receive emails from the system
        /// </summary>
        [StringLength(50)]
        public string Email { get; set; }

        /// <summary>
        /// whether or not the user is allowed to receive emails
        /// </summary>
        [Required]
        public bool EmailEnabled { get; set; }

        /// <summary>
        /// the phone number for the user to rceive SMSes from the system
        /// </summary>
        [StringLength(50)]
        public string Phone { get; set; }

        /// <summary>
        /// can the user receive SMSes from the system
        /// </summary>
        public bool PhoneEnabled { get; set; }

        /// <summary>
        /// should the user rset their password at their next login
        /// </summary>
        public bool PasswordResetRequired { get; set; }

        /// <summary>
        /// how many unsuccessful login attempts has the user mad in a row. used to lock the user automatically
        /// </summary>
        public int LoginAttempts { get; set; }
        public int UserGroupId { get; set; }
        public bool? DepositorEnabled { get; set; }
        public bool UserDeleted { get; set; }
        public bool? IsActive { get; set; }
        
        public bool IsAdUser { get; set; }
        public Guid? ApplicationUserLoginDetailId { get; set; }

        [ForeignKey(nameof(ApplicationUserLoginDetailId))]
        public virtual ApplicationUserLoginDetail ApplicationUserLoginDetail { get; set; }
        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; }
        [ForeignKey(nameof(UserGroupId))]
        public virtual UserGroup UserGroup { get; set; }
        public virtual ICollection<ApplicationUserChangePassword> ApplicationUserChangePasswords { get; set; }
        public virtual ICollection<ApplicationUserLoginDetail> ApplicationUserLoginDetails { get; set; }
        public virtual ICollection<CIT> CITAuthUsers { get; set; }
        public virtual ICollection<CIT> CITStartUsers { get; set; }
        public virtual ICollection<TransactionPosting> TransactionPostingAuthUsers { get; set; }
        public virtual ICollection<TransactionPosting> TransactionPostingInitUsers { get; set; }
        public virtual ICollection<DeviceLock> DeviceLocks { get; set; }
        public virtual ICollection<DeviceLogin> DeviceLogins { get; set; }
        public virtual ICollection<EscrowJam> EscrowJamAuthorisingUsers { get; set; }
        public virtual ICollection<EscrowJam> EscrowJamInitialisingUsers { get; set; }
        public virtual ICollection<PasswordHistory> PasswordHistories { get; set; }
        public virtual ICollection<UserLock> UserLocks { get; set; }
        public virtual ICollection<WebPortalRoleRolesApplicationUserApplicationUser> WebPortalRoleRolesApplicationUserApplicationUsers { get; set; }
        public string FullName => Fname + " " + Lname;
    }
}
