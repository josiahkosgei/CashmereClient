using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
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
            myMediaElement.Position = TimeSpan.FromSeconds(0);
            myMediaElement.Play();
        }

    }
}
