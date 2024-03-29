﻿
//.MenuDeviceStatusMenuATMViewModel




using Caliburn.Micro;
using System;
using System.Windows;
using CashmereDeposit.Models;

namespace CashmereDeposit.ViewModels
{
    internal class MenuDeviceStatusMenuATMViewModel : ATMScreenViewModelBase
    {
        public MenuDeviceStatusMenuATMViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          Conductor<Screen> conductor,
          Screen callingObject)
          : base(screenTitle, applicationViewModel, conductor, callingObject)
        {
            Activated += new EventHandler<ActivationEventArgs>(MenuDeviceStatusATMViewModel_Activated);
            if (!AuthenticationAndAuthorisation.Authenticate(applicationViewModel, ApplicationViewModel.CurrentUser, "DEVICESTATUS_MENU_SHOW", false))
                ErrorText = string.Format("User permission rejected to user {0} for activity {1}, navigating to previous menu.", applicationViewModel.CurrentUser?.Username, "DEVICESTATUS_MENU_SHOW");
            else
                ApplicationViewModel.ShowDialog(this);
        }

        private void MenuDeviceStatusATMViewModel_Activated(object sender, ActivationEventArgs e)
        {
            if (isInitialised)
                return;
            var userPermission = ApplicationViewModel.GetUserPermissionAsync(ApplicationViewModel.CurrentUser, "DEVICE_SUMMARY").ContinueWith(x=>x.Result).Result;
            if (userPermission != null)
            {
                var reportScreenViewModel = new DeviceStatusReportScreenViewModel(Application.Current.FindResource("DeviceStatusScreenTitle") as string, ApplicationViewModel, this);
                object obj = !userPermission.StandaloneAuthenticationRequired ? reportScreenViewModel : new UserLoginViewModel(ApplicationViewModel, ApplicationViewModel, CallingObject, reportScreenViewModel, "DEVICE_SUMMARY", true);
                Screens.Add(new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/presentation-5.png", Application.Current.FindResource("DeviceSummaryCommand_Caption") as string, obj));
            }
            if (ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel.CurrentUser, "DEVICE_CONTROLLER_SHOW"))
                Screens.Add(new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/settings-2.png", Application.Current.FindResource("ShowDeviceControllerCommand_Caption") as string, new AdminButtonCommandATMScreenCommandViewModel(ATMMenuCommandButton.Controller_ShowController, "", ApplicationViewModel, Conductor, this)));
            if (ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel.CurrentUser, "DEVICE_POWER_MENU_SHOW"))
            {
                var screens = Screens;
                if (!(Application.Current.FindResource("DevicePowerMenuScreenTitle_Caption") is string selectionText))
                    selectionText = "Device Power Management";
                if (!(Application.Current.FindResource("DevicePowerMenuScreenTitle_Caption") is string screenTitle))
                    screenTitle = "Device Power Management";
                var menuAtmViewModel = new MenuDeviceShutdownMenuATMViewModel(screenTitle, ApplicationViewModel, Conductor, this);
                var atmSelectionItem = new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/pie-chart-5.png", selectionText, menuAtmViewModel);
                screens.Add(atmSelectionItem);
            }
            isInitialised = true;
        }
    }
}
