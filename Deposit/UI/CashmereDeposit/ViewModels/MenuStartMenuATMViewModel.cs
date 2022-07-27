
//.MenuStartMenuATMViewModel




using Caliburn.Micro;
using System.Windows;
using CashmereDeposit.Models;

namespace CashmereDeposit.ViewModels
{
    internal class MenuStartMenuATMViewModel : ATMScreenViewModelBase
    {
        public MenuStartMenuATMViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          Conductor<Screen> conductor,
          Screen callingObject)
          : base(screenTitle, applicationViewModel, conductor, callingObject)
        {
            Screens.Add(new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/money.png", Application.Current.FindResource("UserModeButtonText") as string, null));
            Screens.Add(new ATMSelectionItem<object>("", "", null));
            Screens.Add(new ATMSelectionItem<object>("", "", null));
            Screens.Add(new ATMSelectionItem<object>("", "", null));
            Screens.Add(new ATMSelectionItem<object>("", "", null));
            Screens.Add(new ATMSelectionItem<object>("", "", null));
            Screens.Add(new ATMSelectionItem<object>("{AppDir}/Resources/Icons/Main/locked-6.png", Application.Current.FindResource("AdminModeButtonText") as string, new MenuCITManagementATMViewModel(Application.Current.FindResource("AdminOptionScreenTitle") as string, ApplicationViewModel, Conductor, this)));
        }
    }
}
