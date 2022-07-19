
//.FormFieldViewModel




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

    public string DataEntryLabel => _dataEntryLabel;

    public string DataEntryTextbox
    {
        get => _textboxText;
        set
      {
        _textboxText = value;
        NotifyOfPropertyChange("TextBoxText");
      }
    }

    public string ErrorMessageTextBlock
    {
        get => _errorMessageTextBlock;
        set
      {
        _errorMessageTextBlock = value;
         NotifyOfPropertyChange( () => ErrorMessageTextBlock);
      }
    }

    public string ValidatedText
    {
        get => _validatedText;
        set
      {
        _validatedText = value;
         NotifyOfPropertyChange( () => ValidatedText);
      }
    }

    public string CancelButton_Caption { get; set; }

    public string NextButton_Caption
    {
        get => _nextButton_Caption;
        set
      {
        _nextButton_Caption = value;
         NotifyOfPropertyChange( () => NextButton_Caption);
      }
    }

    public string BackButton_Caption
    {
        get => _backButton_Caption;
        set
      {
        _backButton_Caption = value;
         NotifyOfPropertyChange( () => BackButton_Caption);
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
      var str = Field.Validate(s);
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
