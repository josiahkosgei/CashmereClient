
//.MenuUserManagementATMViewModel




using Caliburn.Micro;
using System;
using System.Net.Http;
using Cashmere.Library.CashmereDataAccess;

using CashmereDeposit.Models;

namespace CashmereDeposit.ViewModels
{
    internal class MenuUserManagementATMViewModel : ATMScreenViewModelBase
    {
        public MenuUserManagementATMViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          Conductor<Screen> conductor,
          Screen callingObject)
          : base(screenTitle, applicationViewModel, conductor, callingObject)
        {
            Activated += new EventHandler<ActivationEventArgs>((o, args) =>
                MenuUserManagementATMViewModel_Activated(o, args));
            if (!AuthenticationAndAuthorisation.Authenticate(applicationViewModel, ApplicationViewModel.CurrentUser, "USER_MANAGEMENT_MENU_SHOW", false))
                ErrorText = string.Format("User permission rejected to user {0} for activity {1}, navigating to previous menu.", applicationViewModel.CurrentUser?.Username, "USER_MANAGEMENT_MENU_SHOW");
            else
                ApplicationViewModel.ShowDialog(this);
        }

        private void MenuUserManagementATMViewModel_Activated(object sender, ActivationEventArgs e)
        {
            if (isInitialised)
                return;
            if (!ApplicationViewModel.CurrentUser.IsAdUser && ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel.CurrentUser, "USER_CHANGE_PASSWORD"))
            {
                using (new DepositorDBContext())
                    Screens.Add(new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/pin-code.png",
                        ApplicationViewModel.CashmereTranslationService.TranslateSystemText(
                            nameof(MenuUserManagementATMViewModel_Activated), "sys_User_ChangePasswordCommand_Caption",
                            "Change Password", ApplicationViewModel.CurrentLanguage),
                        new UserChangePasswordFormViewModel(ApplicationViewModel, ApplicationViewModel.CurrentUser,
                            null, Conductor, CallingObject, CallingObject)));
            }
            isInitialised = true;
        }
    }
}
