using Newtonsoft.Json;

namespace OpenMLTD.ThankYouSir.LocalDns {
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class DnsConfig {

        [JsonConstructor]
        internal DnsConfig() {
        }

        [JsonProperty(PropertyName = "local_ip")]
        public string LocalIP { get; private set; }

        [JsonProperty(PropertyName = "redirect")]
        public string[] RedirectPatterns { get; private set; }

    }
}