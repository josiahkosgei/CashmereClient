using Caliburn.Micro;
using System;
using System.Windows;
using CashmereDeposit.Models;
using CashAccSysDeviceManager;

namespace CashmereDeposit.ViewModels
{
    internal class MenuCITManagementATMViewModel : ATMScreenViewModelBase
    {
        public MenuCITManagementATMViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          Conductor<Screen> conductor,
          Screen callingObject)
          : base(screenTitle, applicationViewModel, conductor, callingObject)
        {
            Activated += new EventHandler<ActivationEventArgs>(MenuCITManagementATMViewModel_Activated);
            if (!AuthenticationAndAuthorisation.Authenticate(applicationViewModel, ApplicationViewModel.CurrentUser, "CIT_MENU_SHOW", false))
                ErrorText = string.Format("User permission rejected to user {0} for activity {1}, navigating to previous menu.", applicationViewModel.CurrentUser?.Username, "CIT_MENU_SHOW");
            else
                ApplicationViewModel.ShowDialog(this);
        }

        private void MenuCITManagementATMViewModel_Activated(object sender, ActivationEventArgs e)
        {
            if (isInitialised)
                return;
            if (ApplicationViewModel.DeviceManager.DeviceManagerMode == DeviceManagerMode.NONE && ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel?.CurrentUser, "CIT_START"))
            {
                var userLoginViewModel1 = new UserLoginViewModel(ApplicationViewModel, ApplicationViewModel, CallingObject, new CITFormViewModel(ApplicationViewModel, Conductor, this, true), "CIT_AUTHORISER", splitAuthorise: true);
                if (!(Application.Current.FindResource("StartCITCommand_Caption") is string selectionText))
                    selectionText = "Start CIT";
                var userLoginViewModel2 = userLoginViewModel1;
                Screens.Add(new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/safebox-4.png", selectionText, userLoginViewModel2));
            }
            var reportScreenViewModel = new CITReportScreenViewModel(Application.Current.FindResource("CITListScreenTitle") as string, ApplicationViewModel, this);
            object obj = ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel.CurrentUser, "CIT_LIST_VIEW") ? reportScreenViewModel : new UserLoginViewModel(ApplicationViewModel, ApplicationViewModel, CallingObject, reportScreenViewModel, "CIT_LIST_VIEW");
            Screens.Add(new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/presentation-5.png", Application.Current.FindResource("CITListCommand_Caption") as string, obj));
            isInitialised = true;
        }
    }
}
