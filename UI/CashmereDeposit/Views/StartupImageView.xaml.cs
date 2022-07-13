
// Type: CashmereDeposit.Views.StartupImageView
using System;
using System.Windows.Controls;
using System.Windows.Markup;

namespace CashmereDeposit.Views
{
  public partial class StartupImageView : UserControl, IComponentConnector
  {
      public string CopyrightInfo { get; }

    public StartupImageView()
    {
        InitializeComponent();
        CopyrightInfo = $"Copyright © 2018 - {DateTime.Now:yyyy} Maniwa Technologies Ltd. All rights reserved.";
    }
  }
}
