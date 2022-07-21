
//.AdminButtonCommandATMScreenCommandViewModel




using Caliburn.Micro;
using System;
using System.ComponentModel;

namespace CashmereDeposit.ViewModels
{
    internal class AdminButtonCommandATMScreenCommandViewModel : ATMScreenViewModelBase
    {
        private bool _singleActivation = true;
        private bool isActivated;
        private BackgroundWorker statusWorker = new()
        {
            WorkerReportsProgress = false
        };

        private ATMMenuCommandButton Command { get; set; }

        public AdminButtonCommandATMScreenCommandViewModel(
          ATMMenuCommandButton command,
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          Conductor<Screen> conductor,
          Screen callingObject,
          bool singleActivation = true)
          : base(screenTitle, applicationViewModel, conductor, callingObject)
        {
            Command = command;
            _singleActivation = singleActivation;
            Activated += new EventHandler<ActivationEventArgs>(AdminShowControllerATMScreenCommandViewModel_Activated);
            statusWorker.DoWork += new DoWorkEventHandler(StatusWorker_DoWork);
            statusWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(StatusWorker_RunWorkerCompleted);
        }

        private void AdminShowControllerATMScreenCommandViewModel_Activated(
          object sender,
          ActivationEventArgs e)
        {
            if (statusWorker.IsBusy)
                return;
            statusWorker.RunWorkerAsync();
        }

        private void StatusWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            switch (Command)
            {
                case ATMMenuCommandButton.Controller_ShowController:
                    ApplicationViewModel.ShowController();
                    break;
                case ATMMenuCommandButton.Shutdown_PC_Restart:
                    ApplicationViewModel.ShutdownPC(ShutdownCommand.RESTART, "Cashmere GUI on behalf of user " + ApplicationViewModel.CurrentUser?.Username);
                    break;
                case ATMMenuCommandButton.Shutdown_PC_LogOff:
                    ApplicationViewModel.ShutdownPC(ShutdownCommand.LOGOFF, "Cashmere GUI on behalf of user " + ApplicationViewModel.CurrentUser?.Username);
                    break;
            }
        }

        private void StatusWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Conductor.ActivateItemAsync(CallingObject);
        }
    }
}
