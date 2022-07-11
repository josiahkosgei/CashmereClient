
// Type: CashmereDeposit.Models.Notifier

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


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
