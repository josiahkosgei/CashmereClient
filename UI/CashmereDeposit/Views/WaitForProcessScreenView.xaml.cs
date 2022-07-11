
// Type: CashmereDeposit.Views.WaitForProcessScreenView

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace CashmereDeposit.Views
{
  public partial class WaitForProcessScreenView : UserControl, IComponentConnector
  {
    public WaitForProcessScreenView()
    {
        InitializeComponent();
    }

    private void myMediaElement_Loaded(object sender, RoutedEventArgs e)
    {
        myMediaElement.Play();
    }

    private void myMediaElement_MediaEnded(object sender, RoutedEventArgs e)
    {
      myMediaElement.Stop();
      myMediaElement.Position = TimeSpan.FromSeconds(0.0);
      myMediaElement.Play();
    }
  }
}
