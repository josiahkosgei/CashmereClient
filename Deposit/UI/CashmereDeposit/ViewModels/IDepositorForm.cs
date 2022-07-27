using System.Collections.Generic;
using System.Threading.Tasks;

namespace CashmereDeposit.ViewModels
{
    public interface IDepositorForm
    {
        void SelectFormField(int fieldID);

        void SelectFormField(FormListItem field);

        void FormHome(bool success);

        void FormClose(bool success);

        List<FormListItem> GetFields();

        Task<string> SaveForm();

        int FormValidation();

        string GetErrors();
    }
}
