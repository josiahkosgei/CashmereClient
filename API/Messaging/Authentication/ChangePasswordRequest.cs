using Newtonsoft.Json;
using System;
namespace Cashmere.API.Messaging.Authentication
{
    public class ChangePasswordRequest : APIDeviceRequestBase
    {
        [JsonIgnore]
        private object ToStringWithHiddenLock = new object();

        [JsonProperty(Required = Required.Always)]
        public Guid UserId { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Username { get; set; }

        public bool ShouldSerializeOldPassword() => SerializeHidden;

        [JsonProperty(Required = Required.Always)]
        public string OldPassword { get; set; }

        public bool ShouldSerializeNewPassword() => SerializeHidden;

        [JsonProperty(Required = Required.Always)]
        public string NewPassword { get; set; }

        public bool ShouldSerializeConfirmPassword() => SerializeHidden;

        [JsonProperty(Required = Required.Always)]
        public string ConfirmPassword { get; set; }

        public override string ToStringWithHidden()
        {
            lock (ToStringWithHiddenLock)
            {
                SerializeHidden = true;
                string str = JsonConvert.SerializeObject(this);
                SerializeHidden = false;
                return str;
            }
        }

        [JsonIgnore]
        private bool SerializeHidden { get; set; }
    }
}
