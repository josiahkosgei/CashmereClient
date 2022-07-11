
// Type: CashmereDeposit.ViewModels.PermissionRequiredResult

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using Cashmere.Library.CashmereDataAccess.Entities;

namespace CashmereDeposit.ViewModels
{
  public class PermissionRequiredResult
  {
    public bool LoginSuccessful { get; set; }

    public ApplicationUser ApplicationUser { get; set; }
  }
}
