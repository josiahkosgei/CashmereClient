using CashmereDeposit.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace CashmereDeposit.UserControls
{
    public partial class SummaryScreen : UserControl, IComponentConnector
    {
        public SummaryScreen(List<SummaryListItem> boundList)
        {
            InitializeComponent();
            SummaryListBox.ItemsSource = boundList;
        }

        private void OnManipulationBoundaryFeedback(
          object sender,
          ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

    }
}
