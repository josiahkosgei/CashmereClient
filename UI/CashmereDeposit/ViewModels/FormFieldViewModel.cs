
// Type: CashmereDeposit.ViewModels.FormFieldViewModel

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using Caliburn.Micro;
using System;
using System.Linq.Expressions;
using CashmereDeposit.Interfaces;

namespace CashmereDeposit.ViewModels
{
  public class FormFieldViewModel : Conductor<Screen>, IShell
  {
    private string _dataEntryLabel;
    private string _textboxText;
    private string _errorMessageTextBlock;
    private string _validatedText;
    private string _nextButton_Caption;
    private string _backButton_Caption;

    public IDepositorForm Form { get; set; }

    private FormListItem Field { get; set; }

    public string DataEntryLabel
    {
        get { return _dataEntryLabel; }
    }

    public string DataEntryTextbox
    {
        get { return _textboxText; }
        set
      {
        _textboxText = value;
        NotifyOfPropertyChange("TextBoxText");
      }
    }

    public string ErrorMessageTextBlock
    {
        get { return _errorMessageTextBlock; }
        set
      {
        _errorMessageTextBlock = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => ErrorMessageTextBlock));
      }
    }

    public string ValidatedText
    {
        get { return _validatedText; }
        set
      {
        _validatedText = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => ValidatedText));
      }
    }

    public string CancelButton_Caption { get; set; }

    public string NextButton_Caption
    {
        get { return _nextButton_Caption; }
        set
      {
        _nextButton_Caption = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => NextButton_Caption));
      }
    }

    public string BackButton_Caption
    {
        get { return _backButton_Caption; }
        set
      {
        _backButton_Caption = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => BackButton_Caption));
      }
    }

    public string GetFirstPageButton_Caption { get; set; }

    public string GetPreviousPageButton_Caption { get; set; }

    public string GetNextPageButton_Caption { get; set; }

    public string GetLastPageButton_Caption { get; set; }

    public FormFieldViewModel(IDepositorForm form, FormListItem field)
    {
      Form = form;
      Field = field;
      DataEntryTextbox = field.DataTextBoxLabel;
      _dataEntryLabel = field.DataLabel;
      NextButton_Caption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("FormFieldViewModel.Constructor", "sys_NextButton_Caption", "Next");
      BackButton_Caption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("FormFieldViewModel.Constructor", "sys_BackButton_Caption", "Back");
    }

    public void Validate()
    {
        Validate(DataEntryTextbox);
    }

    public void Validate(string s)
    {
      ErrorMessageTextBlock = null;
      string str = Field.Validate(s);
      if (str == null)
      {
        Field.DataTextBoxLabel = s;
        if ((Field.FormListItemType & FormListItemType.PASSWORD) > FormListItemType.NONE)
        {
          Field.ValidatedText = "********";
        }
        else
        {
          Field.ValidatedText = Field.DataTextBoxLabel;
          Field.ErrorMessageTextBlock = "";
        }
        Form.FormHome(true);
      }
      else
        ErrorMessageTextBlock = str;
    }

    public void Back()
    {
        Form.FormHome(false);
    }
  }
}
