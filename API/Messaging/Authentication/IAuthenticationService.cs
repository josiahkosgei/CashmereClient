using System.Threading.Tasks;

namespace Cashmere.API.Messaging.Authentication
{
  public interface IAuthenticationService
  {
    Task<AuthenticationResponse> AuthenticateAsync(
      AuthenticationRequest request);

    Task<ChangePasswordResponse> ChangePasswordAsync(
      ChangePasswordRequest request);
  }
}
