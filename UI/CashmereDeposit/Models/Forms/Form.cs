using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Caliburn.Micro;

namespace CashmereDeposit.Models.Forms
{
  public class Form : Screen
  {
    private string id;
    private string name;
    private string errorMessage;
    private string fullInstructionText;

    public string ID
    {
      get => id;
      set
      {
        id = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => ID));
      }
    }

    public string Name
    {
      get => name;
      set
      {
        name = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => Name));
      }
    }

    public List<FormItem> FormItems { get; set; }

    public string ErrorMessage
    {
      get => errorMessage;
      set
      {
        errorMessage = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => ErrorMessage));
      }
    }

    public string FullInstructionText
    {
      get => fullInstructionText;
      set
      {
        fullInstructionText = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => FullInstructionText));
      }
    }

    public Dictionary<string, ClientSideValidation> ClientSideValidation { get; set; }
  }
}
