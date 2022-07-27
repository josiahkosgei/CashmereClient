
//.FormListboxFieldViewModel




using System.Collections.Generic;

namespace CashmereDeposit.ViewModels
{
    internal class FormListboxFieldViewModel : FormFieldViewModel
    {
        private string selected;

        public List<string> ItemList { get; set; }

        public string SelectedItemList
        {
            get => selected;
            set
            {
                selected = value;
                NotifyOfPropertyChange(nameof(SelectedItemList));
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
