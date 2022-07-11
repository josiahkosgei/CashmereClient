using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities;

public class WebPortalLogin
{
    public WebPortalLogin()
    {
        ApplicationUserLoginDetails = new HashSet<ApplicationUserLoginDetail>();
    }

    [Key]
    public Guid Id { get; set; }
    public DateTime? LogDate { get; set; }
    public Guid? ApplicationUserLoginDetailId { get; set; }
    public int? WebPortalLoginAction { get; set; }
    public bool? Success { get; set; }
    public bool? IsActive { get; set; }
    public bool? ChangePassword { get; set; }
    [StringLength(200)]
    public string Message { get; set; }
    [StringLength(50)]
    public string SessionID { get; set; }
    [StringLength(50)]
    public string SFBegone { get; set; }
    [StringLength(128)]
    public string Hash { get; set; }

    [ForeignKey(nameof(ApplicationUserLoginDetailId))]
    public virtual ApplicationUserLoginDetail ApplicationUserLoginDetail { get; set; }
    public virtual ICollection<ApplicationUserLoginDetail> ApplicationUserLoginDetails { get; set; }
}
