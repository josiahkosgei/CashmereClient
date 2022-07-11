using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("WebPortalLogin")]
    [Index(nameof(ApplicationUserLoginDetail), Name = "iApplicationUserLoginDetail_WebPortalLogin")]
    [Index(nameof(SessionId), Name = "iSessionID_WebPortalLogin", IsUnique = true)]
    public partial class WebPortalLogin
    {
        public WebPortalLogin()
        {
            ApplicationUserLoginDetails = new HashSet<ApplicationUserLoginDetail>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LogDate { get; set; }
        public Guid? ApplicationUserLoginDetail { get; set; }
        public int? WebPortalLoginAction { get; set; }
        public bool? Success { get; set; }
        public bool? IsActive { get; set; }
        public bool? ChangePassword { get; set; }
        [StringLength(200)]
        public string Message { get; set; }
        [Column("SessionID")]
        [StringLength(50)]
        public string SessionId { get; set; }
        [Column("SFBegone")]
        [StringLength(50)]
        public string Sfbegone { get; set; }
        [StringLength(128)]
        public string Hash { get; set; }

        [ForeignKey(nameof(ApplicationUserLoginDetail))]
        [InverseProperty("WebPortalLogins")]
        public virtual ApplicationUserLoginDetail ApplicationUserLoginDetailNavigation { get; set; }
        [InverseProperty("LastLoginLogEntryNavigation")]
        public virtual ICollection<ApplicationUserLoginDetail> ApplicationUserLoginDetails { get; set; }
    }
}
