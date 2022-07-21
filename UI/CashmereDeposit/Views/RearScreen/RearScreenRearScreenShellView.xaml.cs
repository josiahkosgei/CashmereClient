using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace CashmereDeposit.Views.V3
{
    public partial class RearScreenShellView : Window, IComponentConnector
    {
        internal ContentControl ActiveItem;
        private bool _contentLoaded;

        public RearScreenShellView() => InitializeComponent();

        private void Window_Loaded(object sender, RoutedEventArgs e) => (sender as Window).WindowState = WindowState.Maximized;
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;
            _contentLoaded = true;
        }

        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((FrameworkElement)target).Loaded += new RoutedEventHandler(Window_Loaded);
                    break;
                case 2:
                    ActiveItem = (ContentControl)target;
                    break;
                default:
                    _contentLoaded = true;
                    break;
            }
        }
    }
}
