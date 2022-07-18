
// Type: CashmereDeposit.Views.SplashScreenView




using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using CashmereDeposit.ViewModels;

namespace CashmereDeposit.Views
{
  public partial class SplashScreenView : UserControl, IComponentConnector
  {
      private int videoLoopCount = 0;

      public SplashScreenView() => this.InitializeComponent();

      public SplashScreenViewModel viewModel => this.DataContext as SplashScreenViewModel;

      public void SplashScreenTouched(object sender, RoutedEventArgs e) => this.viewModel.ApplicationViewModel.CloseDialog(true);

      private void myMediaElement_Loaded(object sender, RoutedEventArgs e) => this.myMediaElement.Play();

      private void myMediaElement_MediaEnded(object sender, RoutedEventArgs e)
      {
          ApplicationViewModel.Log.TraceFormat(this.GetType().Name, "MediaEnded", "SplashScreen", "Loop {0} complete", (object) (this.videoLoopCount + 1));
          this.myMediaElement.Stop();
          this.myMediaElement.Position = TimeSpan.FromSeconds(0.0);
          this.myMediaElement.Play();
          ++this.videoLoopCount;
      }
  }
}
