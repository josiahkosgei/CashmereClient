
//.ThankYouScreenViewModel




using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace CashmereDeposit.ViewModels
{
  [Guid("E9A4A910-EAFC-4C1B-946B-4BBFBA1E6188")]
  public class ThankYouScreenViewModel : DepositorCustomerScreenBaseViewModel
  {
    private DispatcherTimer dispTimer = new(DispatcherPriority.Send, Application.Current.Dispatcher);

    public ThankYouScreenViewModel(
      string screenTitle,
      ApplicationViewModel applicationViewModel,
      bool required = false)
      : base(screenTitle, applicationViewModel, required, false)
    {
      CanNext = true;
      dispTimer.Interval = TimeSpan.FromSeconds(ApplicationViewModel.DeviceConfiguration.THANK_YOU_TIMEOUT);
      dispTimer.Tick += new EventHandler(dispTimer_Tick);
      dispTimer.IsEnabled = true;
    }

    private void dispTimer_Tick(object sender, EventArgs e)
    {
        Next();
    }

    public void Next()
    {
      dispTimer.Stop();
      lock (ApplicationViewModel.NavigationLock)
      {
        if (!CanNext)
          return;
        CanNext = false;
        var backgroundWorker = new BackgroundWorker();
        backgroundWorker.WorkerReportsProgress = false;
        backgroundWorker.DoWork += new DoWorkEventHandler(StatusWorker_DoWork);
        backgroundWorker.RunWorkerAsync();
      }
    }

    private void StatusWorker_DoWork(object sender, DoWorkEventArgs e)
    {
        ApplicationViewModel.EndSession();
    }
  }
}
