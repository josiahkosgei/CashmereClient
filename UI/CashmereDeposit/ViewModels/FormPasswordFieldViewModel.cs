
// Type: CashmereDeposit.ViewModels.FormPasswordFieldViewModel

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System;
using System.Linq.Expressions;

namespace CashmereDeposit.ViewModels
{
  internal class FormPasswordFieldViewModel : FormFieldViewModel
  {
    private string _passwordLabel_Caption;

    public string PasswordLabel_Caption
    {
        get { return _passwordLabel_Caption; }
        private set
      {
        _passwordLabel_Caption = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => PasswordLabel_Caption));
      }
    }

    public FormPasswordFieldViewModel(IDepositorForm form, FormListItem field)
      : base(form, field)
    {
        PasswordLabel_Caption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText(
            "FormPasswordFieldViewModel.PasswordLabel_Caption", "sys_PasswordLabel_Caption", "Password");
    }

    public new void Validate()
    {
        base.Validate();
    }
  }
}
