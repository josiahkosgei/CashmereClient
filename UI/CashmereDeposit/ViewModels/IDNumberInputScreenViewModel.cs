
//.IDNumberInputScreenViewModel




using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CashmereDeposit.ViewModels
{
    [Guid("085D1E91-E970-4A62-8353-253C35E0D8E8")]
    public class IDNumberInputScreenViewModel : CustomerPrepopReferenceScreenBase
    {
        public IDNumberInputScreenViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required = false)
          : base(applicationViewModel?.CurrentTransaction?.IDNumber, screenTitle, applicationViewModel, required)
        {
        }

        public void Cancel()
        {
            ApplicationViewModel.CancelSessionOnUserInput();
        }

        public void Back()
        {
            ApplicationViewModel.CurrentTransaction.IDNumber = CustomerInput;
            ApplicationViewModel.NavigatePreviousScreen();
        }

        public void Next()
        {
            lock (ApplicationViewModel.NavigationLock)
            {
                if (!CanNext)
                    return;
                CanNext = false;
                ApplicationViewModel.ShowDialog(new WaitForProcessScreenViewModel(ApplicationViewModel));
                var backgroundWorker = new BackgroundWorker
                {
                    WorkerReportsProgress = false
                };
                backgroundWorker.DoWork += new DoWorkEventHandler(StatusWorker_DoWork);
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void StatusWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Validate())
            {
                ApplicationViewModel.CurrentTransaction.IDNumber = CustomerInput;
                ApplicationViewModel.NavigateNextScreen();
            }
            else
            {
                ApplicationViewModel.CloseDialog(false);
                CanNext = true;
            }
        }

        public bool Validate()
        {
            return ClientValidation(CustomerInput);
        }
    }
}
