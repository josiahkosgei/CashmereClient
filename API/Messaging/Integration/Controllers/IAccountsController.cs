// IAccountsController


using Cashmere.API.Messaging.Integration.Validations.AccountNumberValidations;
using Cashmere.API.Messaging.Integration.Validations.ReferenceAccountNumberValidations;

namespace Cashmere.API.Messaging.Integration.Controllers
{
    public interface IAccountsController
    {
        Task<AccountNumberValidationResponse> ValidateAccountNumberAsync(
          AccountNumberValidationRequest request);

        Task<ReferenceAccountNumberValidationResponse> ValidateReferenceAccountNumberAsync(
          ReferenceAccountNumberValidationRequest request);
    }
}
