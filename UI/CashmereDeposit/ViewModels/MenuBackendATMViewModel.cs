
//.MenuBackendATMViewModel




using Caliburn.Micro;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using CashmereDeposit.Models;
using DeviceManager;

namespace CashmereDeposit.ViewModels
{
    [Guid("C0AC72F5-37FF-4CC8-BE9F-842A3787D393")]
    internal class MenuBackendATMViewModel : ATMScreenViewModelBase
    {
        public MenuBackendATMViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          Conductor<Screen> conductor,
          Screen callingObject)
          : base(screenTitle, applicationViewModel, conductor, callingObject)
        {
            Activated += new EventHandler<ActivationEventArgs>(MenuBackendATMViewModel_Activated);
            if (!AuthenticationAndAuthorisation.Authenticate(applicationViewModel, applicationViewModel.CurrentUser, "BACKEND_MENU_SHOW", false))
                ApplicationViewModel.ShowDialog(new UserLoginViewModel(ApplicationViewModel, ApplicationViewModel, CallingObject, this, "BACKEND_MENU_SHOW"));
            else
                ApplicationViewModel.ShowDialog(this);
        }

        private void MenuBackendATMViewModel_Activated(object sender, ActivationEventArgs e)
        {
            if (isInitialised)
                return;
            if (ApplicationViewModel.DeviceManager.DeviceManagerMode == DeviceManagerMode.ESCROW_JAM)
            {
                if (ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel?.CurrentUser, "ESCROWJAM_INITIALISER"))
                {
                    var userLoginViewModel = new UserLoginViewModel(ApplicationViewModel,
                        ApplicationViewModel, CallingObject,
                        new EscrowJamStatusReportScreenViewModel(ApplicationViewModel, Conductor, this, true),
                        "ESCROWJAM_AUTHORISER",splitAuthorise: true);
                    Screens.Add(new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/clear_escrow_jam.png", ApplicationViewModel.CashmereTranslationService.TranslateSystemText(GetType().Name + ".Constructor sys_EscrowJamCommand_Caption", "sys_EscrowJamCommand_Caption", "Clear Escrow Jam"), userLoginViewModel));
                }
            }
            else
            {
                if (ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel.CurrentUser, "DEVICESTATUS_MENU_SHOW"))
                    Screens.Add(new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/pie-chart-5.png", ApplicationViewModel.CashmereTranslationService.TranslateSystemText(nameof(MenuBackendATMViewModel_Activated), "sys_DeviceStatusScreenTitle", "Device Management", ApplicationViewModel.CurrentLanguage), new MenuDeviceStatusMenuATMViewModel(ApplicationViewModel.CashmereTranslationService.TranslateSystemText(nameof(MenuBackendATMViewModel_Activated), "sys_DeviceStatusScreenTitle", "Device Management", ApplicationViewModel.CurrentLanguage), ApplicationViewModel, Conductor, this)));
                if (ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel.CurrentUser, "TRANSACTION_MENU_SHOW"))
                    Screens.Add(new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/change.png", ApplicationViewModel.CashmereTranslationService.TranslateSystemText(nameof(MenuBackendATMViewModel_Activated), "sys_TransactionManagementScreenTitle", "Transactions", ApplicationViewModel.CurrentLanguage), new MenuTransactionATMViewModel(ApplicationViewModel.CashmereTranslationService.TranslateSystemText(nameof(MenuBackendATMViewModel_Activated), "sys_TransactionManagementScreenTitle", "Transactions", ApplicationViewModel.CurrentLanguage), ApplicationViewModel, Conductor, this)));
                if (ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel.CurrentUser, "CIT_MENU_SHOW"))
                    Screens.Add(new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/safebox.png", ApplicationViewModel.CashmereTranslationService.TranslateSystemText(nameof(MenuBackendATMViewModel_Activated), "sys_CITManagementScreenTitle", "CIT Management", ApplicationViewModel.CurrentLanguage), new MenuCITManagementATMViewModel(ApplicationViewModel.CashmereTranslationService.TranslateSystemText(nameof(MenuBackendATMViewModel_Activated), "sys_CITManagementScreenTitle", "CIT Management", ApplicationViewModel.CurrentLanguage), ApplicationViewModel, Conductor, this)));
                if (ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel.CurrentUser, "USER_MANAGEMENT_MENU_SHOW"))
                {
                    var managementAtmViewModel = new MenuUserManagementATMViewModel(ApplicationViewModel.CashmereTranslationService.TranslateSystemText(nameof(MenuBackendATMViewModel_Activated), "sys_UserManagementScreenTitle_Caption", "User Management", ApplicationViewModel.CurrentLanguage), ApplicationViewModel, Conductor, this);
                    var atmSelectionItem = new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/users-1.png", ApplicationViewModel.CashmereTranslationService.TranslateSystemText(nameof(MenuBackendATMViewModel_Activated), "sys_UserManagementScreenTitle_Caption", "User Management", ApplicationViewModel.CurrentLanguage), managementAtmViewModel);
                    if (managementAtmViewModel.Screens.Count() > 0)
                        Screens.Add(atmSelectionItem);
                }
            }
            isInitialised = true;
        }
    }
}
