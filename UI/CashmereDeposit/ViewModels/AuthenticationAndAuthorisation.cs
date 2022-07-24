
//.AuthenticationAndAuthorisation




using Caliburn.Micro;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;


namespace CashmereDeposit.ViewModels
{
    public static class AuthenticationAndAuthorisation
    {
        private static DepositorDBContext _depositorDBContext;

        public static bool Authenticate(
          ApplicationViewModel applicationViewModel,
          ApplicationUser user,
          string activityString,
          bool isAuthorising)
        {
            _depositorDBContext = IoC.Get<DepositorDBContext>();
            if (applicationViewModel.UserPermissionAllowed(user, activityString, isAuthorising))
            {
                _depositorDBContext.SaveChangesAsync().Wait();
                return true;
            }
            _depositorDBContext.SaveChangesAsync().Wait();
            return false;
        }
    }
}
