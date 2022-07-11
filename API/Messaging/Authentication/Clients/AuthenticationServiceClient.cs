using Cashmere.Library.Standard.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Cashmere.API.Messaging.APIClients;

namespace Cashmere.API.Messaging.Authentication.Clients
{
    public class AuthenticationServiceClient : APIClient, IAuthenticationService
    {
        public AuthenticationServiceClient(
          string apiBaseAddress,
          Guid AppID,
          byte[] appKey,
          IConfiguration configuration)
          : base(new CashmereAPILogger(nameof(AuthenticationServiceClient), configuration), apiBaseAddress, AppID, appKey, configuration)
        {
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(
          AuthenticationRequest request) => await SendAsync<AuthenticationResponse>("api/User/Authenticate", request);

        public async Task<ChangePasswordResponse> ChangePasswordAsync(
          ChangePasswordRequest request) => await SendAsync<ChangePasswordResponse>("api/User/ChangePassword", request);
    }
}
