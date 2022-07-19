using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace CashmereDeposit.Views
{
  public partial class CustomerVideoScreen : UserControl, IComponentConnector
  {

    public CustomerVideoScreen() => InitializeComponent();

    private void myMediaElement_Loaded(object sender, RoutedEventArgs e) => myMediaElement.Play();

    private void myMediaElement_MediaEnded(object sender, RoutedEventArgs e) => myMediaElement.Position = TimeSpan.FromSeconds(0.0);
      
  }
}
