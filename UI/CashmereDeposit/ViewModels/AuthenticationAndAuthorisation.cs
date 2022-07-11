
// Type: CashmereDeposit.ViewModels.AuthenticationAndAuthorisation

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;


namespace CashmereDeposit.ViewModels
{
  public static class AuthenticationAndAuthorisation
  {
    public static bool Authenticate(
      ApplicationViewModel applicationViewModel,
      ApplicationUser user,
      string activityString,
      bool isAuthorising)
    {
        using DepositorDBContext DBContext = new DepositorDBContext();
        if (applicationViewModel.UserPermissionAllowed(user, activityString, isAuthorising))
        {
            ApplicationViewModel.SaveToDatabase(DBContext);
            return true;
        }
        ApplicationViewModel.SaveToDatabase(DBContext);
        return false;
    }
  }
}
