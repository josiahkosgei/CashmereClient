using System.ComponentModel;

namespace CashmereDeposit.Models
{
  public abstract class Notifier : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
