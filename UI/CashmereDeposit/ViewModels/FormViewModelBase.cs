
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashmere.Library.CashmereDataAccess;

using CashmereDeposit.Interfaces;

namespace CashmereDeposit.ViewModels
{
  public abstract class FormViewModelBase : Conductor<Screen>,IShell, IDepositorForm, IPermissionRequired
  {
    protected bool isNew;
    public string _formErrorText;
    protected ApplicationViewModel ApplicationViewModel;
    protected DepositorDBContext depositorDbContext = new DepositorDBContext();
    protected DepositorDBContextProcedures depositorContextProcedures;

    public string ScreenTitle { get; set; }

    protected List<FormListItem> Fields { get; set; } = new List<FormListItem>();

    public string FormErrorText
    {
        get { return _formErrorText; }
        set
      {
        _formErrorText = value;
        NotifyOfPropertyChange(nameof (FormErrorText));
      }
    }

    protected Conductor<Screen> Conductor { get; set; }

    protected Screen CallingObject { get; set; }

    public string NextCaption { get; set; }

    public string BackCaption { get; set; }

    public string CancelCaption { get; set; }

    public FormViewModelBase(
      ApplicationViewModel applicationViewModel,
      Conductor<Screen> conductor,
      Screen callingObject,
      bool isNewEntry)
    {
      isNew = isNewEntry;
      ApplicationViewModel = applicationViewModel;
      Conductor = conductor;
      CallingObject = callingObject;
      depositorContextProcedures = new DepositorDBContextProcedures(depositorDbContext);
    }

    public void SelectFormField(int fieldID)
    {
        SelectFormField(Fields[fieldID]);
    }

    public void SelectFormField(FormListItem field)
    {
      if ((field.FormListItemType & FormListItemType.TEXTBOX) > FormListItemType.NONE)
        ActivateItemAsync(new FormFieldViewModel(this, field));
      else if ((field.FormListItemType & FormListItemType.PASSWORD) > FormListItemType.NONE)
      {
        ActivateItemAsync(new FormPasswordFieldViewModel(this, field));
      }
      else
      {
        if ((field.FormListItemType & FormListItemType.LISTBOX) <= FormListItemType.NONE)
          return;
        ActivateItemAsync(new FormListboxFieldViewModel(this, field));
      }
    }

    public void FormHome(bool success)
    {
        ActivateItemAsync(new FormListViewModel(this));
    }

    public List<FormListItem> GetFields()
    {
        return Fields;
    }

    public virtual void FormClose(bool success)
    {
        Conductor.ActivateItemAsync(CallingObject);
    }

    public string GetErrors()
    {
        return FormErrorText;
    }

    public virtual Task<string> SaveForm()
    {
        throw new NotImplementedException();
    }

    public virtual int FormValidation()
    {
        throw new NotImplementedException();
    }

    public virtual void HandleAuthorisationResult(PermissionRequiredResult result)
    {
        throw new NotImplementedException();
    }
  }
}
