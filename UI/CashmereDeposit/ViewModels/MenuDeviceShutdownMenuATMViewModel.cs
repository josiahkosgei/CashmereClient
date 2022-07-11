
// Type: CashmereDeposit.ViewModels.MenuDeviceShutdownMenuATMViewModel

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Windows;
using CashmereDeposit.Models;

namespace CashmereDeposit.ViewModels
{
  internal class MenuDeviceShutdownMenuATMViewModel : ATMScreenViewModelBase
  {
    public MenuDeviceShutdownMenuATMViewModel(
      string screenTitle,
      ApplicationViewModel applicationViewModel,
      Conductor<Screen> conductor,
      Screen callingObject)
      : base(screenTitle, applicationViewModel, conductor, callingObject)
    {
      Activated += new EventHandler<ActivationEventArgs>(MenuDeviceShutdownMenuATMViewModel_Activated);
      if (!AuthenticationAndAuthorisation.Authenticate(applicationViewModel, ApplicationViewModel.CurrentUser, "DEVICESTATUS_MENU_SHOW", false))
        ErrorText = string.Format("User permission rejected to user {0} for activity {1}, navigating to previous menu.", applicationViewModel.CurrentUser?.Username, "DEVICESTATUS_MENU_SHOW");
      else
        ApplicationViewModel.ShowDialog(this);
    }

    private void MenuDeviceShutdownMenuATMViewModel_Activated(object sender, ActivationEventArgs e)
    {
      if (isInitialised)
        return;
      if (ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel.CurrentUser, "DEVICE_POWER_RESTART"))
      {
        IList<ATMSelectionItem<object>> screens = Screens;
        if (!(Application.Current.FindResource("DevicePowerRestartCommand_Caption") is string selectionText))
          selectionText = "Restart PC";
        if (!(Application.Current.FindResource("DevicePowerRestartCommand_Caption") is string screenTitle))
          screenTitle = "Restart PC";
        ApplicationViewModel applicationViewModel = ApplicationViewModel;
        Conductor<Screen> conductor = Conductor;
        AdminButtonCommandATMScreenCommandViewModel commandViewModel = new AdminButtonCommandATMScreenCommandViewModel(ATMMenuCommandButton.Shutdown_PC_Restart, screenTitle, applicationViewModel, conductor, this);
        ATMSelectionItem<object> atmSelectionItem = new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/pie-chart-5.png", selectionText, commandViewModel);
        screens.Add(atmSelectionItem);
      }
      isInitialised = true;
    }
  }
}
