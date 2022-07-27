using Caliburn.Micro;
using Newtonsoft.Json;
using System.ComponentModel;

namespace CashmereDeposit.ViewModels.RearScreen
{
    public class RearScreenShellViewModel : Conductor<Screen>, ICashmereWindowConductor
    {
        private IWindowManager windowManager;

        private RearScreenMainViewModel RearScreenMainViewModel { get; set; }

        public RearScreenShellViewModel(
          IWindowManager theWindowManager,
          ApplicationViewModel applicationViewModel)
        {
            windowManager = theWindowManager;
            ApplicationViewModel = applicationViewModel;
            ApplicationViewModel.PropertyChanged += new PropertyChangedEventHandler(ApplicationViewModel_PropertyChanged);
            RearScreenMainViewModel = new RearScreenMainViewModel(this, applicationViewModel);
            this.ActivateItemAsync((object)RearScreenMainViewModel);
        }

        public ApplicationViewModel ApplicationViewModel { get; }

        private void ApplicationViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(e.PropertyName == "AdminMode"))
                return;
            if (ApplicationViewModel.AdminMode)
            {
                MenuBackendATMViewModel backendAtmViewModel = new MenuBackendATMViewModel("Main Menu", ApplicationViewModel, this, null);
                ApplicationViewModel.ShowDialogBox(new OutOfOrderFatalScreenViewModel());
            }
            else
                CloseDialog(true);
        }

        public void CloseDialog(bool generateScreen = true)
        {
            ApplicationViewModel.Log.InfoFormat(nameof(RearScreenShellViewModel), "Init", nameof(CloseDialog), "showing screen master screen");
            this.ActivateItemAsync((object)RearScreenMainViewModel);
            ApplicationViewModel.CloseDialog(true);
        }

        public void ShowDialog(Screen screen)
        {
            ApplicationViewModel.Log.InfoFormat(nameof(RearScreenShellViewModel), "Init", nameof(ShowDialog), "showing screen {0}", JsonConvert.SerializeObject(screen));
            if (ApplicationViewModel.AdminMode)
            {
                ActivateItemAsync(screen);
            }
            else
            {
                ApplicationViewModel.Log.Warning(nameof(RearScreenShellViewModel), "Invalid  sceen navigation", nameof(ShowDialog), "Not in AdminMode: Cannot show screen " + JsonConvert.SerializeObject(screen));
                ApplicationViewModel.AdminMode = false;
            }
        }

        public void ShowDialogBox(Screen screen)
        {
            ApplicationViewModel.Log.InfoFormat(nameof(RearScreenShellViewModel), "Init", nameof(ShowDialogBox), "showing screen {0}", JsonConvert.SerializeObject(screen));
            ActivateItemAsync(screen);
        }
    }
}
