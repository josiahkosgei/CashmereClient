
// Type: CashmereDeposit.ViewModels.FormListboxFieldViewModel

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.Collections.Generic;

namespace CashmereDeposit.ViewModels
{
  internal class FormListboxFieldViewModel : FormFieldViewModel
  {
    private string selected;

    public List<string> ItemList { get; set; }

    public string SelectedItemList
    {
        get { return selected; }
        set
      {
        selected = value;
        NotifyOfPropertyChange(nameof (SelectedItemList));
        Validate(value);
      }
    }

    public FormListboxFieldViewModel(IDepositorForm form, FormListItem field)
      : base(form, field)
    {
        ItemList = field.ItemList;
    }
  }
}
