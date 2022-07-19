
//.SplashScreenViewModel




using Caliburn.Micro;
using System;
using System.Runtime.InteropServices;

namespace CashmereDeposit.ViewModels
{
  [Guid("3B94AF90-0E62-4D79-89CE-E90DB0610300")]
  public class SplashScreenViewModel : DepositorCustomerScreenBaseViewModel
  {
   
  public string splashVideo => ApplicationViewModel.ApplicationModel.SplashVideoPath;

    public SplashScreenViewModel(
      string screenTitle,
      ApplicationViewModel applicationViewModel,
      bool required = false)
      : base(screenTitle, applicationViewModel, required, false)
    {
        Activated += new EventHandler<ActivationEventArgs>(SplashScreenViewModel_Activated);
    }

    private void SplashScreenViewModel_Activated(object sender, ActivationEventArgs e)
    {
      ApplicationViewModel.AdminMode = false;
      ApplicationViewModel.InitialiseUsersAndPermissions();
      ApplicationViewModel.CurrentApplicationState = Cashmere.Library.Standard.Statuses.ApplicationState.SPLASH;
    }

    public void AdminButton()
    {
      ApplicationViewModel.AdminMode = true;
      var backendAtmViewModel = new MenuBackendATMViewModel("Main Menu", ApplicationViewModel, ApplicationViewModel, this);
    }

    public void Close()
    {
        ApplicationViewModel.SplashScreen_Clicked();
    }
  }
}
