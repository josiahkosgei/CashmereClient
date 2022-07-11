
// Type: CashmereDeposit.ViewModels.FormListItem

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CashmereDeposit.Interfaces;

namespace CashmereDeposit.ViewModels
{
  public class FormListItem : Conductor<Screen>, IShell
  {
    private string _errorMessageTextBlock;

    public string DataLabel { get; set; }

    public string DataTextBoxLabel { get; set; }

    public string ValidatedText { get; set; }

    public string ErrorMessageTextBlock
    {
        get { return _errorMessageTextBlock; }
        set
      {
        _errorMessageTextBlock = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => ErrorMessageTextBlock));
      }
    }

    public List<string> ItemList { get; set; }

    public Func<string, string> Validate { get; set; }

    public FormListItemType FormListItemType { get; set; }
  }
}
