
//.UserMessageScreenViewModel




using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace CashmereDeposit.ViewModels
{
    [Guid("3B5D52F1-6AF5-4AAF-A0B3-EAA130E9DA59")]
    public class UserMessageScreenViewModel : DepositorCustomerScreenBaseViewModel
    {
        private string _message;
        private int _timerDuration;
        private DispatcherTimer dispTimer = new(DispatcherPriority.Send, Application.Current.Dispatcher);

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                ApplicationViewModel.Log?.InfoFormat(GetType().Name, "UserMessageScreen Showing", "Navigation", "Title={0}, Message={1}", ScreenTitle, Message);
            }
        }

        public int TimerDuration
        {
            get => _timerDuration;
            set => _timerDuration = value;
        }

        public UserMessageScreenViewModel(
          string title,
          ApplicationViewModel applicationViewModel,
          int timerDuration = 30,
          bool required = false)
          : base(title, applicationViewModel, required)
        {
            TimerDuration = timerDuration;
            dispTimer.Interval = TimeSpan.FromSeconds(TimerDuration);
            dispTimer.Tick += new EventHandler(dispTimer_Tick);
            dispTimer.IsEnabled = true;
            NextCaption = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText("NextCaption", "sys_Dialog_OK_Caption", "OK");
            ScreenTitle = title;
        }

        private void dispTimer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            ApplicationViewModel.NavigateNextScreen();
        }

        public void Next()
        {
            dispTimer.Stop();
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
            ApplicationViewModel.NavigateNextScreen();
        }
    }
}
