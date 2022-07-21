
// Type: CashmereDeposit.Utils.FormBase




using System.Collections.Generic;
using Caliburn.Micro;

namespace CashmereDeposit.Utils
{
    public class FormBase : PropertyChangedBase
    {
        public List<FormField> Field;
        public FormField CurrentField;

        public string ScreenTitle { get; set; }

        public int currentFieldIndex { get; set; }

        public FormField SelectedField
        {
            get => CurrentField;
            set => CurrentField = value;
        }

        public void SaveForm()
        {
        }

        public void CancelForm()
        {
        }

        public virtual void CloseFormField()
        {
        }
    }
}
