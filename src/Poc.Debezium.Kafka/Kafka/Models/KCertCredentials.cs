using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kafka.Models
{
    public class KCertCredentials
    {
        public const string KEY = "kcert-credentials";

        [JsonPropertyName("kcert_user")]
        [JsonProperty("kcert_user")]
        public string UserName { get; set; }

        [JsonPropertyName("kcert_password")]
        [JsonProperty("kcert_password")]
        public string Password { get; set; }

        [JsonPropertyName("kcert_certificate_password")]
        [JsonProperty("kcert_certificate_password")]
        public string CertificatePassword { get; set; }
    }
}
