
//.MenuTransactionATMViewModel




using Caliburn.Micro;
using System;
using System.Windows;
using CashmereDeposit.Models;

namespace CashmereDeposit.ViewModels
{
    internal class MenuTransactionATMViewModel : ATMScreenViewModelBase
    {
        public MenuTransactionATMViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          Conductor<Screen> conductor,
          Screen callingObject)
          : base(screenTitle, applicationViewModel, conductor, callingObject)
        {
            Activated += new EventHandler<ActivationEventArgs>(MenuTransactionViewModel_Activated);
            if (!AuthenticationAndAuthorisation.Authenticate(applicationViewModel, applicationViewModel.CurrentUser, "TRANSACTION_MENU_SHOW", false))
                ErrorText = string.Format("User permission rejected to user {0} for activity {1}, navigating to previous menu.", applicationViewModel.CurrentUser?.Username, "TRANSACTION_MENU_SHOW");
            else
                ApplicationViewModel.ShowDialog(this);
        }

        private void MenuTransactionViewModel_Activated(object sender, ActivationEventArgs e)
        {
            if (!isInitialised)
            {
                var reportScreenViewModel = new TransactionReportScreenViewModel(Application.Current.FindResource("TransactionListScreenTitle") as string, ApplicationViewModel, this);
                object obj = ApplicationViewModel.UserPermissionAllowed(ApplicationViewModel?.CurrentUser, "TRANSACTION_LIST_VIEW") ? reportScreenViewModel : new UserLoginViewModel(ApplicationViewModel, ApplicationViewModel, CallingObject, reportScreenViewModel, "TRANSACTION_LIST_VIEW");
                Screens.Add(new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/coins.png", Application.Current.FindResource("TransactionlistCommand_Caption") as string, obj));
            }
            isInitialised = true;
        }
    }
}
