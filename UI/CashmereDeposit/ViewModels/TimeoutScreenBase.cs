
//.TimeoutScreenBase




using Caliburn.Micro;
using CashmereDeposit.UserControls;
using System;
using System.Timers;
using System.Windows;
using CashmereDeposit.Interfaces;
using CashmereDeposit.Models;

namespace CashmereDeposit.ViewModels
{
  public class TimeoutScreenBase : Conductor<Screen>, IShell, IDisposable
  {
    private string _screenTitle;
    protected bool EnableIdleTimer;
    protected Timer idleTimer = new Timer();
    protected int SnoozeCount;
    protected double TimeOutInterval;

    public ApplicationViewModel ApplicationViewModel { get; }

    public string ScreenTitle
    {
        get { return _screenTitle; }
        internal set
      {
        _screenTitle = value;
        NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<string>>) (() => ScreenTitle));
      }
    }

    public TimeoutScreenBase(
      string screenTitle,
      ApplicationViewModel applicationViewModel,
      double timeout = 0.0)
    {
        ApplicationViewModel = applicationViewModel;
    }

    private void IdleTimer_Tick(object sender, EventArgs e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            StopIdleTimer();
            string message = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText(
                "IdleTimer_Tick.message", "sys_Dialog_ScreenIdleTimeout_DescriptionText", "Would you like more time?");
            DeviceConfiguration deviceConfiguration = ApplicationViewModel.DeviceConfiguration;
            int timeout = deviceConfiguration != null ? deviceConfiguration.USER_SCREEN_TIMEOUT : 30;
            string title = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText("IdleTimer_Tick.title",
                "sys_Dialog_ScreenIdleTimeout_TitleText", "Still there?");
            if (7 == (int)TimeoutDialogBox.ShowDialog(message, timeout, title, MessageBoxButton.YesNo,
                    MessageBoxImage.Question, MessageBoxResult.Yes))
            {
                EnableIdleTimer = false;
                ApplicationViewModel.Log.Info(GetType().Name + ".IdleTimer_Tick", "ScreenTimeout", "ScreenTimeout",
                    "User clicked NO or ran out of time");
                DoCancelTransactionOnTimeout();
            }
            else
            {
                ApplicationViewModel.Log.Info(GetType().Name + ".IdleTimer_Tick", "ScreenTimeoutSnooze",
                    "ScreenTimeout", "User clicked YES");
                ResetIdleTimerOnSnooze();
            }
        });
    }

    protected virtual void DoCancelTransactionOnTimeout()
    {
      ApplicationViewModel.Log.Info(GetType().Name + ".IdleTimer_Tick", "ScreenTimeoutSnooze", "ScreenTimeout", "User clicked YES");
      ApplicationViewModel.TimeoutSession(ScreenTitle, idleTimer.Interval, string.Format("Snoozed {0:0} times", SnoozeCount));
    }

    public void ResetIdleTimerOnUserInteraction()
    {
        StartIdleTimer();
    }

    private void ResetIdleTimerOnSnooze()
    {
      ++SnoozeCount;
      ApplicationViewModel.Log.InfoFormat(GetType().Name, nameof (ResetIdleTimerOnSnooze), "ScreenTimeout", "Snooze Count = {0:0}", SnoozeCount);
      StartIdleTimer();
    }

    protected void StopIdleTimer()
    {
      if (idleTimer == null)
        return;
      idleTimer.Stop();
      idleTimer.Elapsed -= new ElapsedEventHandler(IdleTimer_Tick);
    }

    protected void StartIdleTimer()
    {
      try
      {
        if (idleTimer == null || !EnableIdleTimer)
          return;
        if (TimeOutInterval <= 0.0)
          throw new ArgumentOutOfRangeException("TimeOutInterval", string.Format("TimeOutInterval must be > 0: found {0}", TimeOutInterval));
        StopIdleTimer();
        idleTimer.Interval = TimeOutInterval * 1000.0;
        idleTimer.Elapsed += new ElapsedEventHandler(IdleTimer_Tick);
        idleTimer.Start();
      }
      catch (Exception ex)
      {
      }
    }

    public virtual void Dispose()
    {
      StopIdleTimer();
      idleTimer.Dispose();
      idleTimer = null;
    }
  }
}
