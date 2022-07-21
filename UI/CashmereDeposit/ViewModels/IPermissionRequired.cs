
//.IPermissionRequired




namespace CashmereDeposit.ViewModels
{
    public interface IPermissionRequired
    {
        void HandleAuthorisationResult(PermissionRequiredResult result);
    }
}
