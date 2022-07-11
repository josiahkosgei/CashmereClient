
using Cashmere.Library.Standard.Logging;
using Cashmere.Library.Standard.Security;
using Cashmere.Library.Standard.Utilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Cashmere.API.Messaging.APIClients
{
    public class APIClient
    {
        private ICashmereAPILogger Log;
        private IConfiguration Configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        private string APIBaseAddress { get; set; }

        private Guid API_ID { get; set; }

        private byte[] API_Key { get; set; }

        private readonly HttpClient _httpClient;
        public APIClient(
          ICashmereAPILogger logger,
          string apiBaseAddress,
          Guid appID,
          byte[] appKey,
          IConfiguration configuration
          )
        {
            if (string.IsNullOrEmpty(apiBaseAddress))
                throw new ArgumentException("Must provide apiBaseAddress", nameof(apiBaseAddress));
            Configuration = configuration;
            _httpClientFactory = IoC.Get<IHttpClientFactory>();
            Log = logger ?? throw new ArgumentNullException(nameof(logger));
            APIBaseAddress = apiBaseAddress;
            API_ID = appID;
            API_Key = appKey;
            _httpClient = _httpClientFactory.CreateClient("CashmereDepositHttpClient");
        }

        public async Task<T> SendAsync<T>(string endpoint, APIMessageBase message)
        {
            T obj;
            try
            {
                /*
                 
                HttpClient httpClient = HttpClientFactory.Create(new DelegatingHandler[1]
                    {
                      (DelegatingHandler) new HMACDelegatingHandler(this.API_ID, this.API_Key)
                    });
                 */


                Log.Trace(message.SessionID, message.MessageID, message.AppName, nameof(APIClient), "Generate Json", nameof(SendAsync), "Converting APIMessage to Json");

                string stringWithHidden = message.ToStringWithHidden();
                string Message = message.ToString();

                Log.Debug(message.SessionID, message.MessageID, message.AppName, nameof(APIClient), "Generate Json", nameof(SendAsync), Message);
                Log.Trace(message.SessionID, message.MessageID, message.AppName, nameof(APIClient), "Generate HTTPMessage content", nameof(SendAsync), "Generating string content");

                Encoding unicode = Encoding.UTF8;
                StringContent stringContent = new StringContent(stringWithHidden, unicode, "application/json");

                stringContent.Headers.Add("SessionID", message.SessionID);
                stringContent.Headers.Add("MessageID", message.MessageID);
                stringContent.Headers.Add("AppName", message.AppName);
                Log.Debug(message.SessionID, message.MessageID, message.AppName, nameof(APIClient), "API TX", nameof(SendAsync), "Sending to {0} >> {1}", APIBaseAddress + endpoint, Message);
                HttpResponseMessage response = await _httpClient.PostAsync(APIBaseAddress + endpoint, stringContent);
                Log.Debug(message.SessionID, message.MessageID, message.AppName, nameof(APIClient), "API Rx", nameof(SendAsync), "Received http response {0}", response.ToString());
                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    Log.Debug(message.SessionID, message.MessageID, message.AppName, nameof(APIClient), "API Rx", nameof(SendAsync), "Received http response {0}", str);
                    T responseObject = JsonConvert.DeserializeObject<T>(str);
                    Guid? nullable = responseObject is APIMessageBase apiMessageBase ? new Guid?(apiMessageBase.AppID) : new Guid?();
                    Guid appId = message.AppID;
                    if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() != appId ? 1 : 0) : 0) : 1) != 0)
                        throw new Exception("Invalid response app id");
                    await ValidateResponse(response, JsonConvert.SerializeObject(responseObject));
                    obj = responseObject;
                }
                else
                {
                    Console.WriteLine("Failed to call the API. HTTP Status: {0}, Reason {1}", response.StatusCode, response.ReasonPhrase);
                    throw new Exception(string.Format("API Call failed with status code [{0}]: {1}", response.StatusCode, response.ReasonPhrase));
                }
            }
            catch (Exception ex)
            {
                Log.Error(message.SessionID, message.MessageID, message.AppName, nameof(APIClient), nameof(SendAsync), ex.GetType().Name, ex.MessageString());
                throw;
            }
            return obj;
        }

        private async Task ValidateResponse(HttpResponseMessage response, string responseString)
        {
            string rawAuthzHeader = response.Headers.GetValues("hmacauth").FirstOrDefault();
            string[] strArray = !string.IsNullOrEmpty(rawAuthzHeader) ? GetAutherizationHeaderValues(rawAuthzHeader) : throw new Exception("Invalid Response");
            string app_id = strArray != null ? strArray[0] : throw new Exception("Invalid Response");
            string incomingBase64Signature = strArray[1];
            string nonce = strArray[2];
            string requestTimeStamp = strArray[3];
            if (!await IsValidResponseAsync(response, responseString, app_id, incomingBase64Signature, nonce, requestTimeStamp))
                throw new Exception("Invalid Response");
        }

        private string[] GetAutherizationHeaderValues(string rawAuthzHeader)
        {
            string[] strArray = rawAuthzHeader.Split(':');
            return strArray.Length == 4 ? strArray : null;
        }

        private async Task<bool> IsValidResponseAsync(
          HttpResponseMessage response,
          string responseString,
          string app_id,
          string incomingBase64Signature,
          string nonce,
          string requestTimeStamp)
        {
            string absoluteUri = response.RequestMessage.RequestUri.AbsoluteUri;
            string method = response.RequestMessage.Method.Method;
            string str = CashmereHashing.SHA256WithEncode(responseString, Encoding.Unicode);
            string s = string.Format("{0}{1}{2}{3}{4}{5}", app_id, method, absoluteUri, requestTimeStamp, nonce, str);
            byte[] apiKey = API_Key;
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            bool flag;
            using HMACSHA256 hmacshA256 = new HMACSHA256(apiKey);
            flag = incomingBase64Signature.Equals(hmacshA256.ComputeHash(bytes).ToBase64String(), StringComparison.Ordinal);
            return flag;
        }

        private static byte[] ComputeHash(byte[] content)
        {
            byte[] numArray = null;
            if (content.Length != 0)
                numArray = CashmereHashing.ComputeHash(content, SHA256.Create());
            return numArray;
        }
    }
}
