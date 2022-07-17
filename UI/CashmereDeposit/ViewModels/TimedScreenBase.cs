
//.TimedScreenBase




using System;
using System.Windows.Threading;

namespace CashmereDeposit.ViewModels
{
  public abstract class TimedScreenBase
  {
    protected DispatcherTimer idleTimer = new();

    protected TimeSpan TimeSpan { get; }

    public TimedScreenBase(int timeSpan)
    {
      TimeSpan = TimeSpan.FromSeconds(timeSpan);
      if (timeSpan <= 0)
        return;
      idleTimer.Interval = TimeSpan;
      idleTimer.Tick += new EventHandler(IdleTimer_Tick);
      idleTimer.IsEnabled = true;
    }

    protected abstract void IdleTimer_Tick(object sender, EventArgs e);

    protected abstract void IdleTimer_Reset(object sender, EventArgs e);
  }
}
