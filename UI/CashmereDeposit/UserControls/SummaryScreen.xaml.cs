
// Type: CashmereDeposit.UserControls.SummaryScreen

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using CashmereDeposit.Models;

namespace CashmereDeposit.UserControls
{
    public partial class SummaryScreen : UserControl, IComponentConnector
    {
        public SummaryScreen(List<SummaryListItem> boundList)
        {
            InitializeComponent();
            SummaryListBox.ItemsSource = (IEnumerable)boundList;
        }

        private void OnManipulationBoundaryFeedback(
            object sender,
            ManipulationBoundaryFeedbackEventArgs e) => e.Handled = true;
    }
}
