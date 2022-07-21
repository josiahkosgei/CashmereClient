
// Type: Cashmere.Library.Standard.Security.HMACDelegatingHandler


using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cashmere.Library.Standard.Security
{
    public class HMACDelegatingHandler : DelegatingHandler
    {
        private Guid APPId;
        private byte[] APIKey;

        public HMACDelegatingHandler(Guid aPPId, byte[] apiKey)
        {
            APPId = aPPId;
            APIKey = apiKey;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
          HttpRequestMessage request,
          CancellationToken cancellationToken)
        {
            string requestUri = request.RequestUri.AbsoluteUri;
            string requestHttpMethod = request.Method.Method;
            string content = await request.Content.ReadAsStringAsync();
            request.Headers.Add("hmacAuth", APIHashing.GetAuthHeader(APPId, requestUri, requestHttpMethod, APIKey, content));
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
