// Decompiled with JetBrains decompiler
// Type: CashmereDeposit.Views.SplashScreenView
// Assembly: CashmereDeposit, Version=6.6.5.7, Culture=neutral, PublicKeyToken=null
// MVID: 7BC9C8AC-6829-47FD-BEA6-5232B37B7616
// Assembly location: C:\DEV\maniwa\bak\New folder\6.0 - Demo\CashmereDeposit.exe

using CashmereDeposit.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace CashmereDeposit.Views
{
  public partial class SplashScreenView : UserControl, IComponentConnector
  {
    private int videoLoopCount = 0;

    public SplashScreenView() => InitializeComponent();

    public SplashScreenViewModel viewModel => DataContext as SplashScreenViewModel;

    public void SplashScreenTouched(object sender, RoutedEventArgs e) => viewModel.ApplicationViewModel.CloseDialog(true);

    private void myMediaElement_Loaded(object sender, RoutedEventArgs e) => myMediaElement.Play();

    private void myMediaElement_MediaEnded(object sender, RoutedEventArgs e)
    {
      ApplicationViewModel.Log.TraceFormat(GetType().Name, "MediaEnded", "SplashScreen", "Loop {0} complete", (object) (videoLoopCount + 1));
      myMediaElement.Stop();
      myMediaElement.Position = TimeSpan.FromSeconds(0.0);
      myMediaElement.Play();
      ++videoLoopCount;
    }
    
  }
}
