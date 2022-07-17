
//.PermissionRequiredResult




using Cashmere.Library.CashmereDataAccess.Entities;

namespace CashmereDeposit.ViewModels
{
  public class PermissionRequiredResult
  {
    public bool LoginSuccessful { get; set; }

    public ApplicationUser ApplicationUser { get; set; }
  }
}
